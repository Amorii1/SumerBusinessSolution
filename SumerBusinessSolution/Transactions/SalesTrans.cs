using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Transactions
{
    public class SalesTrans : ISalesTrans
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SalesTrans(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public InvStockQty InvStockQty { get; set; }
        public Warehouse ShowRoom { get; set; }

        // this function will create a bill for the first time
        public async Task<string> CreateBill(BillHeader Header, List<BillItems> BillItems, int WhId, string Type, int? OldBhId)
        {
            try
            {
                Header.Id = new int();
                Header.CreatedById = GetLoggedInUserId();
                Header.CreatedDataTime = DateTime.Now;
 

                //// getting the unit price of each item in the bill items 
                //foreach (BillItems item in BillItems)
                //{
                //    item.ProdId = _db.ProdInfo.FirstOrDefault(pr => pr.ProdCode == item.ProdInfo.ProdCode).Id;
                ////    first check if qty enough in the store room before proceeding
                //    bool CheckQty = CheckQtyInWh(item.ProdId ?? 0, WhId, item.Qty);

                //    if (CheckQty == false)
                //    {
                //        return "Error! لا توجد كمية كافية للبيع";
                //    }

                //}

                //// price before discount
                //Header.TotalAmt = TotalAmt;

                // incase there is a discount
                //double TotalNetAmt = Header.TotalAmt - Header.Discount;
                //Header.TotalNetAmt = TotalNetAmt;

                if (Header.TotalNetAmt == Header.PaidAmt)
                {
                    Header.Status = SD.Completed;
                }
                else
                {
                    Header.Status = SD.OpenBill;
                }


                _db.BillHeader.Add(Header);

                await _db.SaveChangesAsync();

                // updatinga customer Acc 
                double DebtAmt = Header.TotalNetAmt - Header.PaidAmt;
                UpdateCustomerAcc(Header.CustId ?? 0, Header.PaidAmt, DebtAmt, "New");

                // add a new payment to bill payments table 
                AddBillPayment(Header.CustId ?? 0, Header.Id, Header.PaidAmt);


                // Creating Bill items
                foreach (BillItems item in BillItems)
                {
                    item.ProdId = _db.ProdInfo.FirstOrDefault(pr => pr.ProdCode == item.ProdInfo.ProdCode).Id;

                    BillItems Bill = new BillItems
                    {
                        HeaderId = Header.Id,
                        ProdId = item.ProdId,
                        Qty = item.Qty,
                        WhId = WhId,
                        UnitPrice = item.UnitPrice,
                        TotalAmt = item.UnitPrice * item.Qty,
                        Note = item.Note
                    };

                    // decrease stock qty of that item 
                    DecreaseStockQty(Bill.ProdId ?? 0, WhId, Bill.Qty);

                    // create inv transaction 
                    CreateInvTransaction(Bill.ProdId ?? 0, WhId, Bill.Qty, Header.Id, SD.Sales);
                    _db.BillItems.Add(Bill);

                }


                await _db.SaveChangesAsync();


                // if this function being used to edit an exisiting bill, then the old bill will be deleted 
                if (Type == "Edit")
                {
                    DeleteBill(OldBhId ?? 0).GetAwaiter().GetResult();
                    return "تم التعديل على الفاتورة";

                }

                return "تمت اضافة فاتورة مبيعات جديدة";
            }

            catch
            {
                return "Error! حصل خطأ لم يتم اضافة الفاتورة";
            }
        }


        public async Task<string> MakePaymentOnBill(int HeaderId, double NewPaymentAmt)
        {
            try
            {
                BillHeader Header = _db.BillHeader.FirstOrDefault(h => h.Id == HeaderId);

                CustAcc Acc = _db.CustAcc.FirstOrDefault(ac => ac.CustId == Header.CustId);

                // updating bill header will the payment
                Header.PaidAmt += NewPaymentAmt;

                // if the bill paid all, change status to completed
                if (Header.TotalNetAmt == Header.PaidAmt)
                {
                    Header.Status = SD.Completed;
                }

                // updating customer Acc
                //Acc.Paid += PaidAmt;
                //Acc.Debt -= PaidAmt;
                UpdateCustomerAcc(Header.CustId ?? 0, NewPaymentAmt, NewPaymentAmt, "Old");

                // add a new payment to bill payments table 
                AddBillPayment(Header.CustId ?? 0, Header.Id, NewPaymentAmt);

                await _db.SaveChangesAsync();

                return "تمت عملية الدفع";
            }
            catch (Exception ex)
            {
                return "لم تتم عملية الدفع";
            }
        }

        public async Task<string> MakePaymentToAcc(int CustId, double NewPaymentAmt)
        {
            try
            {
                CustAcc Acc = _db.CustAcc.FirstOrDefault(ac => ac.CustId == CustId);


                // updating customer Acc
                //Acc.Paid += PaidAmt;
                //Acc.Debt -= PaidAmt;
                UpdateCustomerAcc(CustId, NewPaymentAmt, NewPaymentAmt, "Old");

                await _db.SaveChangesAsync();

                return "تمت عملية الدفع";
            }
            catch (Exception ex)
            {
                return "لم تتم عملية الدفع";
            }
        }

        // this function will close a bill manually 
        public async Task<string> CloseBillManually(int HeaderId)
        {
            try
            {
                BillHeader Header = _db.BillHeader.FirstOrDefault(h => h.Id == HeaderId);
                Header.Status = SD.Completed;
                await _db.SaveChangesAsync();

                return "تم اغلاق الفاتورة بنجاح";
            }
            catch
            {
                return "حصل خطأ لم يتم اغلاق الفاتورة";
            }

        }


        public async Task<string> DeleteBill(int HeaderId)
        {
            try
            {


                BillHeader Header = _db.BillHeader.FirstOrDefault(h => h.Id == HeaderId);
                List<BillItems> BillItemList = _db.BillItems.Where(i => i.HeaderId == HeaderId).ToList();

                // reverting customer Acc 
                double DebtAmt = Header.TotalNetAmt - Header.PaidAmt;
                RevertCustomerAcc(Header.CustId ?? 0, Header.PaidAmt, DebtAmt);

                // remove bill payment of bill payments table 
                DeleteBillPayment(Header.Id);

                // Creating Bill items
                foreach (BillItems Item in BillItemList)
                {

                    // revert stock qty of that item 
                    RevertStockQty(Item.ProdId ?? 0, Item.WhId, Item.Qty);


                    //   _db.BillItems.Add(Bill);

                }
                // remove inv transaction list from InvTransaction Table
                DeleteInvTransaction(HeaderId, SD.Sales);

                // removing bill item list
                _db.BillItems.RemoveRange(BillItemList);

                //remove header
                _db.BillHeader.RemoveRange(Header);

                await _db.SaveChangesAsync();


                return "تم حذف الفاتورة";
            }

            catch
            {
                return "Error! حصل خطأ لم يتم حذف الفاتورة";
            }
        }

        // this function will update customer account manually, only for admin
        public async Task<string> UpdateCustomerAccManually(int CustId, double Payment, double Debt)
        {
            try
            {
                CustAcc Acc = _db.CustAcc.FirstOrDefault(ac => ac.CustId == CustId);

                if (Payment > 0)
                {
                    Acc.Paid = Payment;
                }

                if (Debt > 0)
                {
                    Acc.Debt = Debt;
                }


                await _db.SaveChangesAsync();

                return "تم تعديل الحساب";
            }
            catch
            {
                return "Error! حصل خطأ لم يتم تعديل الحساب";
            }

        }


        // External Billing

        // this function will create an external bill.. external bill is a bill issues for a customers
        // for purchasing items that do not belong to the company. basically it is supplied by another or a 
        // third party company. creating external bills will have no effect on Inventory at all

        public async Task<string> CreateExternalBill(ExternalBillHeader ExternalHeader, List<ExternalBillItems> ExternalBill, int WhId, string Type, int? OldBhId)
        {
            try
            {
                ExternalHeader.Id = new int();
                ExternalHeader.CreatedById = GetLoggedInUserId();
                ExternalHeader.CreatedDataTime = DateTime.Now;

                double TotalAmt = 0;
                int ProdId;
                // getting the unit price of each item in the bill items 
                foreach (ExternalBillItems item in ExternalBill)
                {
                    ProdId = 0;
                    try
                    {
                        if (Type == "Edit")
                        {
                            ProdId = _db.ProdInfo.FirstOrDefault(pr => pr.ProdName == item.ProdInfo.ProdName).Id;

                        }
                        else
                        {
                            ProdId = _db.ProdInfo.FirstOrDefault(pr => pr.ProdCode == item.ProdInfo.ProdCode).Id;

                        }

                    }
                    catch
                    {

                    }

                    if (ProdId > 0)
                    {
                        item.ProdId = ProdId;
                        item.IsExternal = false;
                        // first check if qty enough in the store room before proceeding
                        bool CheckQty = CheckQtyInWh(item.ProdId ?? 0, WhId, item.Qty);
                        // ExternalHeader.HasExternalProd = false;
                        if (CheckQty == false)
                        {
                            return "Error! لا توجد كمية كافية للبيع";
                        }
                    }
                    else
                    {
                        // to indicase if the product of this line is an external product
                        // which means its bought from another shop and has no effect on 
                        // inventory 
                        item.IsExternal = true;
                        ExternalHeader.HasExternalProd = true;
                    }
                    TotalAmt += item.TotalAmt;
                }

                // price before discount
                ExternalHeader.TotalAmt = TotalAmt;

                // incase there is a discount
                TotalAmt = TotalAmt - ExternalHeader.Discount;
                ExternalHeader.TotalNetAmt = TotalAmt;

                if (TotalAmt == ExternalHeader.PaidAmt)
                {
                    ExternalHeader.Status = SD.Completed;
                }
                else
                {
                    ExternalHeader.Status = SD.OpenBill;
                }


                _db.ExternalBillHeader.Add(ExternalHeader);

                await _db.SaveChangesAsync();

                // updatinga customer Acc 
                double DebtAmt = ExternalHeader.TotalNetAmt - ExternalHeader.PaidAmt;
                UpdateCustomerAcc(ExternalHeader.CustId ?? 0, ExternalHeader.PaidAmt, DebtAmt, "New");

                // add a new payment to bill payments table 
                AddExternalBillPayment(ExternalHeader.CustId ?? 0, ExternalHeader.Id, ExternalHeader.PaidAmt);


                // Creating Bill items
                foreach (ExternalBillItems item in ExternalBill)
                {

                    ExternalBillItems Bill = new ExternalBillItems
                    {
                        HeaderId = ExternalHeader.Id,
                        // ProdId = item.ProdId,
                        Qty = item.Qty,
                        UnitPrice = item.UnitPrice,
                        WhId = WhId,
                        TotalAmt = item.UnitPrice * item.Qty,
                        // IsExternal = false,
                        Note = item.Note
                    };

                    if (item.IsExternal == false)
                    {
                        Bill.ProdId = item.ProdId;
                        Bill.IsExternal = false;
                        // decrease stock qty of that item 
                        DecreaseStockQty(Bill.ProdId ?? 0, WhId, Bill.Qty);

                        // create inv transaction 
                        CreateInvTransaction(Bill.ProdId ?? 0, WhId, Bill.Qty, ExternalHeader.Id, SD.Sales);
                        _db.ExternalBillItems.Add(Bill);
                    }
                    else
                    {
                        if (Type == "Edit")
                        {
                            Bill.ProdName = item.ProdName;
                        }
                        else
                        {
                            Bill.ProdName = item.ProdInfo.ProdCode;
                        }

                        Bill.IsExternal = true;
                        //Bill.ProdName = Bill.ProdId.ToString();
                        _db.ExternalBillItems.Add(Bill);
                    }

                }


                await _db.SaveChangesAsync();

                if (Type == "Edit")
                {
                    DeleteExternalBill(OldBhId ?? 0).GetAwaiter().GetResult();
                    return "تم التعديل على الفاتورة";

                }

                return "تمت اضافة فاتورة مبيعات جديدة";
            }

            catch (Exception x)
            {
                try
                {
                    _db.ExternalBillHeader.Remove(ExternalHeader);
                    _db.ExternalBillItems.RemoveRange(ExternalBill);
                    _db.SaveChanges();
                }
                catch
                {

                }
                return "Error! حصل خطأ لم يتم اضافة الفاتورة";
            }
        }

        public async Task<string> CreateExternalBillOLD(ExternalBillHeader ExternalHeader, List<ExternalBillItems> ExternalBill, int WhId)
        {
            try
            {
                ShowRoom = _db.Warehouse.Include(wh => wh.WhType).FirstOrDefault(wh => wh.WhType.Type == "StoreRoom");

                ExternalHeader.CreatedById = GetLoggedInUserId();
                ExternalHeader.CreatedDataTime = DateTime.Now;

                double TotalAmt = 0;

                // getting the unit price of each item in the bill items 
                foreach (ExternalBillItems item in ExternalBill)
                {

                    TotalAmt += item.TotalAmt;
                }

                // price before discount
                ExternalHeader.TotalAmt = TotalAmt;

                // in case there is a discount
                TotalAmt = TotalAmt - ExternalHeader.Discount;
                ExternalHeader.TotalNetAmt = TotalAmt;

                if (TotalAmt == ExternalHeader.PaidAmt)
                {
                    ExternalHeader.Status = SD.Completed;
                }
                else
                {
                    ExternalHeader.Status = SD.OpenBill;
                }


                _db.ExternalBillHeader.Add(ExternalHeader);

                await _db.SaveChangesAsync();

                // updatinga customer Acc 
                double DebtAmt = ExternalHeader.TotalNetAmt - ExternalHeader.PaidAmt;
                UpdateCustomerAcc(ExternalHeader.CustId ?? 0, ExternalHeader.PaidAmt, DebtAmt, "New");

                // add a new payment to bill payments table 
                // AddBillPayment(Header.CustId ?? 0, Header.Id, Header.PaidAmt);


                // Creating Bill items
                foreach (ExternalBillItems item in ExternalBill)
                {
                    ExternalBillItems Bill = new ExternalBillItems
                    {
                        HeaderId = ExternalHeader.Id,
                        ProdName = item.ProdName,
                        Qty = item.Qty,
                        UnitPrice = item.UnitPrice,
                        TotalAmt = item.UnitPrice * item.Qty,
                        Note = item.Note
                    };
                    _db.ExternalBillItems.Add(Bill);
                }


                await _db.SaveChangesAsync();


                return "تمت اضافة فاتورة مبيعات جديدة";
            }

            catch
            {
                return "حصل خطأ لم يتم اضافة الفاتورة";
            }
        }

        // this function will close an external bill manually 
        public async Task<string> CloseExternalBillManually(int ExternalHeaderId)
        {
            try
            {
                ExternalBillHeader ExternalHeader = _db.ExternalBillHeader.FirstOrDefault(h => h.Id == ExternalHeaderId);
                ExternalHeader.Status = SD.Completed;
                await _db.SaveChangesAsync();

                return "تم اغلاق الفاتورة بنجاح";
            }
            catch
            {
                return "Error! حصل خطأ لم يتم اغلاق الفاتورة";
            }

        }

        // Make payment on external bills
        public async Task<string> MakePaymentOnExternalBill(int ExternalHeaderId, double NewPaymentAmt)
        {
            try
            {
                ExternalBillHeader ExternalHeader = _db.ExternalBillHeader.FirstOrDefault(h => h.Id == ExternalHeaderId);

                CustAcc Acc = _db.CustAcc.FirstOrDefault(ac => ac.CustId == ExternalHeader.CustId);

                // updating bill header will the payment
                ExternalHeader.PaidAmt += NewPaymentAmt;

                // if the bill paid all, change status to completed
                if (ExternalHeader.TotalNetAmt == ExternalHeader.PaidAmt)
                {
                    ExternalHeader.Status = SD.Completed;
                }

                // updating customer Acc
                //Acc.Paid += PaidAmt;
                //Acc.Debt -= PaidAmt;
                UpdateCustomerAcc(ExternalHeader.CustId ?? 0, NewPaymentAmt, NewPaymentAmt, "Old");

                // add a new payment to bill payments table 
                AddExternalBillPayment(ExternalHeader.CustId ?? 0, ExternalHeader.Id, NewPaymentAmt);

                await _db.SaveChangesAsync();

                return "تمت عملية الدفع";
            }
            catch (Exception ex)
            {
                return "لم تتم عملية الدفع";
            }
        }

        public async Task<string> DeleteExternalBill(int HeaderId)
        {
            try
            {

                ExternalBillHeader ExternalHeader = _db.ExternalBillHeader.FirstOrDefault(h => h.Id == HeaderId);
                List<ExternalBillItems> BillItemList = _db.ExternalBillItems.Where(i => i.HeaderId == HeaderId).ToList();

                // reverting customer Acc 
                double DebtAmt = ExternalHeader.TotalNetAmt - ExternalHeader.PaidAmt;
                RevertCustomerAcc(ExternalHeader.CustId ?? 0, ExternalHeader.PaidAmt, DebtAmt);

                // remove bill payment of bill payments table 
                DeleteBillPayment(ExternalHeader.Id);

                // Creating Bill items
                foreach (ExternalBillItems Item in BillItemList)
                {
                    if (Item.IsExternal == false)
                    {
                        // revert stock qty of that item 
                        RevertStockQty(Item.ProdId ?? 0, Item.WhId, Item.Qty);

                    }
                }
                // remove inv transaction list from InvTransaction Table
                DeleteInvTransaction(HeaderId, SD.Sales);

                // removing bill item list
                _db.ExternalBillItems.RemoveRange(BillItemList);

                //remove header
                _db.ExternalBillHeader.RemoveRange(ExternalHeader);

                await _db.SaveChangesAsync();


                return "تم حذف الفاتورة";
            }

            catch
            {
                return "Error! حصل خطأ لم يتم حذف الفاتورة";
            }
        }


        // Helping functions // 
        private string GetLoggedInUserId()
        {
            var ClaimId = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var Claim = ClaimId.FindFirst(ClaimTypes.NameIdentifier);
            string UserId = Claim.Value;
            return UserId;
        }



        // updating customer Accs 
        private void UpdateCustomerAcc(int CustId, double Paid, double Debt, string Status)
        {
            CustAcc Acc = _db.CustAcc.FirstOrDefault(cu => cu.CustId == CustId);

            Acc.Paid += Paid;

            // if new means new bill and will add more debit (in case customer didnt make full payment)
            if (Status == "New")
            {
                Acc.Debt += Debt;
            }

            else // this will be used when a customer makes a new payment on existing bill, so debt will be minus
            {
                if (Acc.Debt > 0)
                {
                    Acc.Debt -= Debt;

                }

            }

        }

        // this function will be used to update customer acc when a bill is edited 
        private void UpdateCustAccOnBillEdit(int CustId, double NewPayment, double OldPayment, double NewDebt, double OldDebt)
        {
            CustAcc Acc = _db.CustAcc.FirstOrDefault(cu => cu.CustId == CustId);

            // update new payment
            if (NewPayment > 0)
            {
                Acc.Paid -= OldPayment;
                Acc.Paid += NewPayment;
            }

            //update new debt
            if (NewDebt > 0)
            {
                Acc.Debt += OldDebt;
                Acc.Debt -= NewDebt;
            }



        }

        // Revert customer acc, when bill is cancelled
        private void RevertCustomerAcc(int CustId, double Paid, double Debt)
        {
            CustAcc Acc = _db.CustAcc.FirstOrDefault(cu => cu.CustId == CustId);

            if (Acc != null)
            {
                // reverting both amount of customer's account
                Acc.Paid -= Paid;
                Acc.Debt -= Debt;
            }

        }


        // checking item qty before sales 
        private bool CheckQtyInWh(int ProdId, int WhId, double Qty)
        {
            InvStockQty = _db.InvStockQty.FirstOrDefaultAsync(inv => inv.ProdId == ProdId & inv.WhId == WhId).GetAwaiter().GetResult();

            double StockQty = InvStockQty.Qty;

            if (StockQty >= Qty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // this function will update bill payment table
        private void AddBillPayment(int CustId, int BillHeaderId, double Paid)
        {
            BillPayment NewPayment = new BillPayment
            {
                CustId = CustId,
                BillHeaderId = BillHeaderId,
                PaidAmt = Paid,
                CreatedDateTime = DateTime.Now,
                CreatedById = GetLoggedInUserId()
            };
            _db.BillPayment.Add(NewPayment);

        }


        // will delete bill payment when a bill is deleted
        private void DeleteBillPayment(int BillHeaderId)
        {
            BillPayment Payment = _db.BillPayment.FirstOrDefault(p => p.BillHeaderId == BillHeaderId);
            if (Payment != null)
            {
                _db.BillPayment.Remove(Payment);
            }
        }


        // this function will update External bill payment table
        private void AddExternalBillPayment(int CustId, int ExternalBillHeaderId, double Paid)
        {
            ExternalBillPayment NewPayment = new ExternalBillPayment
            {
                CustId = CustId,
                ExternalBillHeaderId = ExternalBillHeaderId,
                PaidAmt = Paid,
                CreatedDateTime = DateTime.Now,
                CreatedById = GetLoggedInUserId()
            };
            _db.ExternalBillPayment.Add(NewPayment);

        }


        // this function will decrease the stock qty
        private void DecreaseStockQty(int ProdId, int WhId, double Qty)
        {
            InvStockQty = _db.InvStockQty.FirstOrDefaultAsync(inv => inv.ProdId == ProdId & inv.WhId == WhId).GetAwaiter().GetResult();
            if (InvStockQty != null)
            {
                InvStockQty.Qty -= Qty;
            }

        }

        // reverting stock qty when a bill is cancelled
        private void RevertStockQty(int ProdId, int WhId, double Qty)
        {
            InvStockQty = _db.InvStockQty.FirstOrDefaultAsync(inv => inv.ProdId == ProdId & inv.WhId == WhId).GetAwaiter().GetResult();
            if (InvStockQty != null)
            {
                InvStockQty.Qty += Qty;
            }

        }


        // add a sale transaction inside invTransaction table 
        private void CreateInvTransaction(int ProdId, int? WhId, double Qty, int RefTransId, string TransType)
        {
            InvTransaction InvTrans = new InvTransaction
            {
                ProdId = ProdId,
                WhId = WhId,
                Qty = Qty,
                TransType = TransType,
                RefTransId = RefTransId,
                CreatedById = GetLoggedInUserId(),
                CreatedDateTime = DateTime.Now
            };
            _db.InvTransaction.Add(InvTrans);
        }

        // remove inventory trans list when bill is deleted
        private void DeleteInvTransaction(int RefTransId, string TransType)
        {
            List<InvTransaction> TransList = _db.InvTransaction
                 .Where(t => t.RefTransId == RefTransId & t.TransType == TransType).ToList();

            if (TransList.Count > 0)
            {
                _db.InvTransaction.RemoveRange(TransList);
            }
        }
    }
}
