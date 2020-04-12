using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using SumerBusinessSolution.Utility;
using Microsoft.AspNetCore.SignalR;
using SumerBusinessSolution.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.Hubs;
namespace SumerBusinessSolution.Transactions
{
    public class InventoryTrans : IInventoryTrans
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public IncomingGood IncomingGood { get; set; }

        public InvStockQty InvStockQty { get; set; }
        public InvTransaction InvTransaction { get; set; }
        public IList<Warehouse> WarehouseList { get; set; }
        public ProdInfo ProdInfo { get; set; }

        public InvTransfer InvTransfer { get; set; }
        public InvTransferHeader InvTransferHeader { get; set; }
        public List<InvTransfer> InvTransferList { get; set; }


        public InventoryTrans(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;

        }

        // This Function used when created a new product for the first time. 
        //it checks if Product Code is used before or not "Product Code must be used once only"
        // If used before then return False, otherwise return True
        public async Task<bool> CheckProdCodeExist(string ProdCode)
        {
            ProdInfo = await _db.ProdInfo.FirstOrDefaultAsync(prod => prod.ProdCode == ProdCode);
            if(ProdInfo == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // This function used for the first time a product is created. 
        // First will open a balance qty of a selected warehouse in (WhStockQty) when a product is created, and the rest of the 
        // warehouses, a record will be created with zero Qty
        public bool CreateProdInWh(int ProdId, int WhId, double OpenQty)
        {
            WarehouseList = _db.Warehouse.Where(wh=> wh.Active == true).ToList();

            try
            {
                //Iterating through a list of all warehouses
                foreach (Warehouse Wh in WarehouseList)
                {
                    if(Wh.Id == WhId)
                    {
                        // this line will create an open qty balance at the selected warehouse
                        CreateInvTransaction(ProdId, Wh.Id, OpenQty, SD.OpenBalance);
                        CreateInvStockQty(ProdId, Wh.Id, OpenQty).GetAwaiter().GetResult();
                        
                    }
                    else
                    {
                        // the rest of the warehouses will be with zero qty
                        CreateInvStockQty(ProdId, Wh.Id, 0).GetAwaiter().GetResult();
                    }
                    
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

     

        public async Task<string> CreateIncomingGoods(int SelectedWhId ,List<IncomingGood> IG)
        {
            try
            {
                DateTime InDateTime = DateTime.Now;
                string sqlFormattedDate = InDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                int ProdId;
                 
                foreach (var income in IG)
                {
                    ProdId = _db.ProdInfo.FirstOrDefault(pro => pro.ProdCode == income.ProdInfo.ProdCode).Id;
                    
                   IncomingGood IncomingGood = new IncomingGood
                    {
                        ProdId = ProdId,
                        WhId = SelectedWhId,
                        Qty = income.Qty,
                        Note = income.Note,
                        CreatedById = GetLoggedInUserId(),

                        CreatedDateTime = InDateTime
                    };

                    //adding new incoming goods to the list
                    _db.IncomingGood.Add(IncomingGood);

                    //updating Qty of InvStockQty
                    ChangeStockQty(ProdId, SelectedWhId, income.Qty, "In");

                    // creating transaction 
                    CreateInvTransaction(ProdId, SelectedWhId, income.Qty, SD.Incoming);
                }
 
                // Save changes
                await _db.SaveChangesAsync();

                return "تمت اضافة المواد الواردة";
            }
            catch (Exception ex)
            {
                return "Error! حصل خطا. لم تتم اضافة المواد";
            }
        }

        //This function deletes incoming Goods based on the id of the selected Incoming Goods. 
        // Notice this function will decrease the qty of the deleted item 
        //  also will delete Inv Transaction 
        public async Task<bool> DeleteIncomingGoods(int IgId)
        {
            try
            {
                IncomingGood IncomingGood = _db.IncomingGood.FirstOrDefault(ig => ig.Id == IgId);

                int ProdId = IncomingGood.ProdId.GetValueOrDefault();
                int WhId = IncomingGood.WhId.GetValueOrDefault();
                double Qty = IncomingGood.Qty;
               // DateTime TransDate = IncomingGood.CreatedDateTime;

                //updating Qty of InvStockQty
                ChangeStockQty(ProdId, WhId, Qty, "Out");

                // Delete add trans ID
                DeleteInvTransaction(ProdId, WhId, Qty, SD.Incoming);

                _db.IncomingGood.Remove(IncomingGood);

                // Save changes
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return false;
            }
        }


        public async Task<string> CreateInvTransfer(int? FromWhId, int? ToWhId, List<InvTransfer> InvTrans)
        {

            InvTransferHeader NewHeader = new InvTransferHeader
            {
                FromWhId = FromWhId,
                ToWhId = ToWhId,
                TransferStatus = SD.Approved,
                CreatedById = GetLoggedInUserId(),
                CreatedDateTime = DateTime.Now

            };
            _db.InvTransferHeader.Add(NewHeader);
            await _db.SaveChangesAsync();

            int ProdId;
            foreach(var InvTr in InvTrans)
            {
                ProdId = _db.ProdInfo.FirstOrDefault(pro => pro.ProdCode == InvTr.ProdInfo.ProdCode).Id;

                // Check if the warehouse has enough qty of that product
                bool CheckQty = CheckQtyInWh(ProdId, FromWhId??0, InvTr.Qty);
                if (CheckQty == false)
                {
                     _db.InvTransferHeader.Remove(NewHeader);
                    await _db.SaveChangesAsync();
                    return "Error! لايوجد كمية كافية للمادة: " + InvTr.ProdInfo.ProdCode;
                }


                InvTransfer InvTransfer = new InvTransfer
                {
                    HeaderId = NewHeader.Id,
                    ProdId = ProdId,
                    Qty = InvTr.Qty,
                    Note = InvTr.Note
                };

                //Decrease Qty of From Warehouse
                ChangeStockQty(ProdId, FromWhId??0, InvTr.Qty, "Out");

                // Increase Qty of To Warehouse 
                ChangeStockQty(ProdId, ToWhId??0, InvTr.Qty, "In");
                _db.InvTransfer.Add(InvTransfer);

                // Create Inv Transaction with Negative Qty of From Warehouse
                CreateInvTransaction(ProdId, FromWhId??0, InvTr.Qty * -1, SD.TransferOut);

                // Create Inv Transaction with Positive Qty of To Warehouse
                CreateInvTransaction(ProdId, ToWhId??0, InvTr.Qty, SD.TransferIn);
            }

            await _db.SaveChangesAsync();

            return "تمت عملية التحويل";

        }

        // This function is called when a Store User wants to transfer from a warehouse to another. So this function
        // Will create a request for the Admin to approve

        public async Task<string> CreateInvTransferRequest(int? FromWhId, int? ToWhId, string Note, List<InvTransfer> InvTrans, IHubContext<NotificationHub> hubContext)
        {
            // Create Transfer Header

            InvTransferHeader NewHeader = new InvTransferHeader
            {
                FromWhId = FromWhId,
                ToWhId = ToWhId,
                TransferStatus = SD.Pending,
                CreatedById = GetLoggedInUserId(),
                CreatedDateTime = DateTime.Now,
                Note = Note

            };
            _db.InvTransferHeader.Add(NewHeader);
            await _db.SaveChangesAsync();

            int ProdId;
            foreach (var InvTr in InvTrans)
            {
                ProdId = _db.ProdInfo.FirstOrDefault(pro => pro.ProdCode == InvTr.ProdInfo.ProdCode).Id;

                // Check if the warehouse has enough qty of that product
                bool CheckQty = CheckQtyInWh(ProdId, FromWhId ?? 0, InvTr.Qty);
                if (CheckQty == false)
                {
                    _db.InvTransferHeader.Remove(NewHeader);
                    await _db.SaveChangesAsync();
                    return "Error! لايوجد كمية كافية للمادة: " + InvTr.ProdInfo.ProdCode;
                }


                InvTransfer InvTransfer = new InvTransfer
                {
                    HeaderId = NewHeader.Id,
                    ProdId = ProdId,
                    Qty = InvTr.Qty,
                    Note = InvTr.Note
                };
                _db.InvTransfer.Add(InvTransfer);
            }

            await _db.SaveChangesAsync();

            await hubContext.Clients.All.SendAsync("NewTransferRequest", _db.Warehouse.Find(NewHeader.FromWhId).WhName, _db.Warehouse.Find(NewHeader.ToWhId).WhName, NewHeader.Id);

            return "تم ارسال طلب التحويل بنجاح";
            //return "Request Added Successfully";

        }


        // Signalr notifications 
        public async Task<IEnumerable<InvTransferHeader>> GetPendingTransferRequests()
        {
            return await _db.InvTransferHeader.Include(c => c.FromWarehouse).Include(c => c.ToWarehouse).Where(c => c.TransferStatus == SD.Pending).ToListAsync();
        }
        //public async Task<string> CreateInvTransferRequest(int ProdId, int FromWhId, int ToWhId, double Qty, string Note)
        //{
        //    // Check if the warehouse has enough qty of that product
        //    bool CheckQty = CheckQtyInWh(ProdId, FromWhId, Qty);

        //    if (CheckQty == false)
        //    {
        //        //return "Error! الكمية غير كافية للتحويل";
        //        return "Error! No enough Qty";

        //    }

        //    // Create Inv Transfer Record with Pending Status
        //    InvTransfer InvTransfer = new InvTransfer
        //    {
        //        ProdId = ProdId,
        //        FromWhId = FromWhId,
        //        ToWhId = ToWhId,
        //        Qty = Qty,
        //        TransferStatus = SD.Pending,
        //        CreatedById = GetLoggedInUserId(),
        //        CreatedDateTime = DateTime.Now,
        //        Note = Note
        //    };

        //    _db.InvTransfer.Add(InvTransfer);
        //    await _db.SaveChangesAsync();

        //    //return "تم ارسال طلب التحويل";
        //    return "Request Added Successfully";

        //}

        // When a transfer request is created by the Store user. Admin will call this function to Approve his 
        // Transfer Request
        public async Task<string> ApproveInvTransferRequest(int ReqId)
        {
            // Get Inventory Object by the ID 
            InvTransferHeader = _db.InvTransferHeader.FirstOrDefault(H => H.Id == ReqId);

            if (InvTransferHeader == null)
            {
                return "هذا الطلب غير موجود";
            }

            InvTransferList = _db.InvTransfer.Where(inv => inv.HeaderId == ReqId).ToList();

            foreach(InvTransfer Inv in InvTransferList)
            {
                int ProdId = Inv.ProdId ?? 0;
                int FromWhId = InvTransferHeader.FromWhId ?? 0;
                int ToWhId = InvTransferHeader.ToWhId ?? 0;
                double Qty = Inv.Qty;

                bool CheckQty = CheckQtyInWh(ProdId, FromWhId, Qty);

                if (CheckQty == false)
                {
                    return "Error! لايوجد كمية كافية للمادة: " + Inv.ProdInfo.ProdCode;
                }

                //Decrease Qty of From Warehouse
                ChangeStockQty(ProdId, FromWhId, Qty, "Out");

                // Increase Qty of To Warehouse 
                ChangeStockQty(ProdId, ToWhId, Qty, "In");

                // Create Inv Transaction with Negative Qty of From Warehouse
                CreateInvTransaction(ProdId, FromWhId, Qty * -1, SD.TransferOut);

                // Create Inv Transaction with Positive Qty of To Warehouse
                CreateInvTransaction(ProdId, ToWhId, Qty, SD.TransferIn);
            }

            // Approve Transfer Requrest

            InvTransferHeader.TransferStatus = SD.Approved;
            InvTransferHeader.ApprovedById = GetLoggedInUserId();

            await _db.SaveChangesAsync();

            return "تمت الموافقة على الطلب بنجاح";
        }

        // Admin can reject Inventory transfer request by using this funtion 
        public async Task<string> RejectInvTransferRequest(int ReqId)
        {

            // Get Inventory Object by the ID 
            InvTransferHeader = _db.InvTransferHeader.FirstOrDefault(H => H.Id == ReqId);

            if (InvTransferHeader == null)
            {
                return "هذا الطلب غير موجود";
            }
            // Reject Transfer Requrest

            InvTransferHeader.TransferStatus = SD.Rejected;

            await _db.SaveChangesAsync();

            return "تم الرفض على الطلب ";
        }

        // user can delete Inventory transfer request Header by using this funtion 
        public async Task<string> DeleteInvTransferRequestHeader(int ReqId)
        {
            // Get Inventory Object by the ID 
            InvTransferHeader = _db.InvTransferHeader.FirstOrDefault(H => H.Id == ReqId);

            if (InvTransferHeader == null)
            {
                return "هذا الطلب غير موجود";
            }
            // Delete Transfer Requrest Header

            _db.InvTransferHeader.Remove(InvTransferHeader);

            await _db.SaveChangesAsync();

            return "تم مسح الطلب ";
        }

        // user can delete Inventory transfer request Line by using this funtion 
        public async Task<string> DeleteInvTransferRequestLine(int LineId)
        {
            // Get Inventory Object by the ID 
            InvTransfer = _db.InvTransfer.FirstOrDefault(Inv => Inv.Id == LineId);

            if (InvTransfer == null)
            {
                return "هذه المادة غير موجود";
            }
            // Delete Transfer Requrest Line

            _db.InvTransfer.Remove(InvTransfer);

            await _db.SaveChangesAsync();

            return "تم مسح المادة ";
        }


        //This funtion will update the stock qty of a product. By admin only
        public async Task<bool> UpdateProdStkQty(int StkId, double Qty)
        {
             
            try
            {
                InvStockQty InvStockQty = _db.InvStockQty.FirstOrDefault(stk => stk.Id == StkId);

                InvStockQty.Qty = Qty;

                // Save changes
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return false;
            }
        }

        // Helping Functions //
        private string GetLoggedInUserId()
        {
            //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //return userId;

            var ClaimId = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var Claim = ClaimId.FindFirst(ClaimTypes.NameIdentifier);
            string UserId = Claim.Value;
            return UserId;
        }

        private void ChangeStockQty(int ProdId, int WhId, double Qty, string OpType)
        {
            InvStockQty = _db.InvStockQty.FirstOrDefaultAsync(inv => inv.ProdId == ProdId & inv.WhId == WhId).GetAwaiter().GetResult();
            if (InvStockQty != null)
            {
                // if OpType == In, increase the Qty. Else Decrease 
                if(OpType == "In")
                {
                    InvStockQty.Qty += Qty;
                }
                else
                {
                    InvStockQty.Qty -= Qty;
                }
                
            }
       
        }
        private void CreateInvTransaction(int ProdId, int? WhId, double Qty, string TransType)
        {
            InvTransaction InvTrans = new InvTransaction
            {
                ProdId = ProdId,
                WhId = WhId,
                Qty = Qty,
                TransType = TransType,
                CreatedById = GetLoggedInUserId(),
                CreatedDateTime = DateTime.Now
            };
            _db.InvTransaction.Add(InvTrans);
        }

        private void DeleteInvTransaction(int ProdId, int WhId, double Qty, string TransType)
        {
            try
            {
                InvTransaction InvTrans = _db.InvTransaction.FirstOrDefault(tr => tr.ProdId == ProdId
                & tr.WhId == WhId & tr.Qty == Qty & tr.TransType == TransType);

                _db.InvTransaction.Remove(InvTrans);
            }
            catch
            {

            }
        
        }

        private bool CheckQtyInWh(int ProdId, int WhId, double Qty)
        {
            InvStockQty = _db.InvStockQty.FirstOrDefaultAsync(inv => inv.ProdId == ProdId & inv.WhId == WhId).GetAwaiter().GetResult();

            if(InvStockQty == null)
            {
                return false;
            }

            double StockQty = InvStockQty.Qty;

            if(StockQty >= Qty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<bool> CreateInvStockQty(int ProdId, int WhId, double Qty)
        {
            // Create Inv Stock Record for the product
            InvStockQty InvStockQty  = new InvStockQty
            {
                ProdId = ProdId,
                WhId = WhId,
                Qty = Qty
            };

            _db.InvStockQty.Add(InvStockQty);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
