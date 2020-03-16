using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sumer.Utility;
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
        public Warehouse StoreRoom { get; set; }
        
        // this function will create a bill for the first time
        public async Task<string> CreateBill(BillHeader Header, List<BillItems> BillItems)
        {
            try
            {
                StoreRoom = _db.Warehouse.Include(wh => wh.WhType).FirstOrDefault(wh => wh.WhType.Type == "StoreRoom"); 

                Header.CreatedById = GetLoggedInUserId();
                Header.CreatedDataTime = DateTime.Now;

                double TotalAmt = 0;

                // getting the unit price of each item in the bill items 
                foreach (BillItems item in BillItems)
                {
                    // first check if qty enough in the store room before proceeding
                    bool CheckQty = CheckQtyInWh(item.ProdInfo.Id, StoreRoom.Id, item.Qty);

                    if(CheckQty == false)
                    {
                        return "لا توجد كمية كافية للبيع";
                    }

                    TotalAmt += item.TotalAmt;
                }

                // price before discount
                Header.TotalAmt = TotalAmt;

                // incase there is a discount
                TotalAmt = TotalAmt - Header.Discount;
                Header.TotalNetAmt = TotalAmt;

                if(TotalAmt == Header.PaidAmt)
                {
                    Header.Status = SD.Completed;
                }
                else
                {
                    Header.Status = SD.OpenBill;
                }
                

                _db.BillHeader.Add(Header);

                // updatinga customer Acc 
                double DebtAmt = Header.TotalNetAmt - Header.PaidAmt;
                UpdateCustomerAcc(Header.CustId ?? 0, Header.PaidAmt, DebtAmt, "New");

                // add a new payment to bill payments table 
                AddBillPayment(Header.CustId ?? 0, Header.Id, Header.PaidAmt);

                await _db.SaveChangesAsync();

                // Creating Bill items
                foreach (BillItems item in BillItems)
                {
                    BillItems Bill = new BillItems
                    {
                        HeaderId = Header.Id,
                        ProdId = item.ProdInfo.Id,
                        Qty = item.Qty,
                        UnitPrice = item.UnitPrice,
                        TotalAmt = item.UnitPrice * item.Qty,
                        Note = item.Note
                    };
                    _db.BillItems.Add(Bill);
                }
                
            
                await _db.SaveChangesAsync();


                return "تمت اضافة فاتورة مبيعات جديدة";
            }

            catch
            {
                return "حصل خطأ لم يتم اضافة الفاتورة";
            }
        }

        // this function will create new Bill payment, used when a customer pays an outstanding bill
        public async Task<string> MakePaymentOnBill(int HeaderId, double NewPaymentAmt)
        {
            try
            {
                BillHeader Header =  _db.BillHeader.FirstOrDefault(h => h.Id == HeaderId);

                CustAcc Acc = _db.CustAcc.FirstOrDefault(ac => ac.CustId == Header.CustId);
                
                // updating bill header will the payment
                Header.PaidAmt += NewPaymentAmt;

                // if the bill paid all, change status to completed
                if(Header.TotalNetAmt == Header.PaidAmt)
                {
                    Header.Status = SD.Completed;
                }

                // updating customer Acc
                //Acc.Paid += PaidAmt;
                //Acc.Debt -= PaidAmt;
                UpdateCustomerAcc(Header.CustId ?? 0, NewPaymentAmt, NewPaymentAmt, "Old");

                // add a new payment to bill payments table 
                AddBillPayment(Header.CustId??0, Header.Id, NewPaymentAmt);
 
                await _db.SaveChangesAsync();

                return "تمت عملية الدفع";
            }
            catch(Exception ex)
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
            if(Status == "New")
            {
                Acc.Debt += Debt;
            }
            else // this will be used when a customer makes a new payment on existing bill, so debt will be minus
            {
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

    }
}
