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
using Sumer.Utility;

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
        // When a product is created it will create a record of this product for each Warehouse with ZERO Qty
        public bool CreateProdInWh(int ProdId)
        {
            WarehouseList = _db.Warehouse.ToList();

            try
            {
                //Iterating through a list of all warehouses
                foreach (Warehouse Wh in WarehouseList)
                {
                    CreateInvStockQty(ProdId, Wh.Id, 0).GetAwaiter().GetResult() ;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        // This function used to create Incoming Goods
        public async Task<bool> CreateIncomingGoods(int WhId, int ProdId, double Qty, string Note)
        {
            try
            {
                // creating new incoming goods
                IncomingGood = new IncomingGood
                {
                    ProdId = ProdId,
                    WhId = WhId,
                    Qty = Qty,
                    Note = Note,
                    CreatedById = GetLoggedInUserId(),
                    CreatedDateTime = DateTime.Now

                };
                 _db.IncomingGood.Add(IncomingGood);

                //updating Qty of InvStockQty
                ChangeStockQty(ProdId, WhId, Qty, "In");

                CreateInvTransaction(ProdId, WhId, Qty, SD.Incoming);
  
                // Save changes
                await _db.SaveChangesAsync();
             
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        // This function used to create inventory Transfer. The transfer is created only For the Admin
        public async Task<bool> CreateInvTransfer(int ProdId, int FromWhId, int ToWhId, double Qty, string Note)
        {
            // Check if the warehouse has enough qty of that product
            bool CheckQty = CheckQtyInWh(ProdId, FromWhId, Qty);

            if(CheckQty == false)
            {
                return false;
            }

            //Decrease Qty of From Warehouse
            ChangeStockQty(ProdId, FromWhId, Qty, "Out");

            // Increase Qty of To Warehouse 
            ChangeStockQty(ProdId, ToWhId, Qty, "In");


            // Create Inv Transaction with Negative Qty of From Warehouse
            CreateInvTransaction(ProdId, FromWhId, Qty*-1, SD.TransferOut);

            // Create Inv Transaction with Positive Qty of To Warehouse
            CreateInvTransaction(ProdId, ToWhId, Qty, SD.TransferIn);

            // Create Inv Transfer Record 
            InvTransfer InvTransfer = new InvTransfer
            {
                ProdId = ProdId,
                FromWhId = FromWhId,
                ToWhId = ToWhId,
                Qty = Qty,
                TransferStatus = SD.Approved,
                CreatedById = GetLoggedInUserId(),
                CreatedDateTime = DateTime.Now, 
                Note = Note
            };

            _db.InvTransfer.Add(InvTransfer);
            await _db.SaveChangesAsync();

            return true;
        }

        // This function is called when a Store User wants to transfer from a warehouse to another. So this function
        // Will create a request for the Admin to approve
        public async Task<bool> CreateInvTransferRequest(int ProdId, int FromWhId, int ToWhId, double Qty, string Note)
        {
            // Check if the warehouse has enough qty of that product
            bool CheckQty = CheckQtyInWh(ProdId, FromWhId, Qty);

            if (CheckQty == false)
            {
                return false;
            }

            // Create Inv Transfer Record with Pending Status
            InvTransfer InvTransfer = new InvTransfer
            {
                ProdId = ProdId,
                FromWhId = FromWhId,
                ToWhId = ToWhId,
                Qty = Qty,
                TransferStatus = SD.Pending,
                CreatedById = GetLoggedInUserId(),
                CreatedDateTime = DateTime.Now,
                Note = Note
            };

            _db.InvTransfer.Add(InvTransfer);
            await _db.SaveChangesAsync();

            return true;
        }

        // When a transfer request is created by the Store user. Admin will call this function to Approve his 
        // Transfer Request
        public async Task<bool> ApproveInvTransferRequest(int ReqId)
        {
    
            // Get Inventory Object by the ID 
            InvTransfer = _db.InvTransfer.FirstOrDefault(inv => inv.Id == ReqId);

            if(InvTransfer == null)
            {
                return false;
            }

            int ProdId = InvTransfer.ProdId ?? 0;
            int FromWhId = InvTransfer.FromWhId ?? 0;
            int ToWhId = InvTransfer.ToWhId ?? 0;
            double Qty = InvTransfer.Qty;

            bool CheckQty = CheckQtyInWh(ProdId, FromWhId, Qty);

            if (CheckQty == false)
            {
                return false;
            }

            //Decrease Qty of From Warehouse
            ChangeStockQty(ProdId, FromWhId, Qty, "Out");

            // Increase Qty of To Warehouse 
            ChangeStockQty(ProdId, ToWhId, Qty, "In");


            // Create Inv Transaction with Negative Qty of From Warehouse
            CreateInvTransaction(ProdId, FromWhId, Qty * -1, SD.TransferOut);

            // Create Inv Transaction with Positive Qty of To Warehouse
            CreateInvTransaction(ProdId, ToWhId, Qty, SD.TransferIn);

            // Approve Transfer Requrest

            InvTransfer.TransferStatus = SD.Approved;
            InvTransfer.ApprovedById = GetLoggedInUserId();

            await _db.SaveChangesAsync();

            return true;
        }

        // Admin can reject Inventory transfer request by using this funtion 
        public async Task<bool> RejectInvTransferRequest(int ReqId)
        {

            // Get Inventory Object by the ID 
            InvTransfer = _db.InvTransfer.FirstOrDefault(inv => inv.Id == ReqId);

            if (InvTransfer == null)
            {
                return false;
            }
            // Reject Transfer Requrest

            InvTransfer.TransferStatus = SD.Rejected;

            await _db.SaveChangesAsync();

            return true;
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
        private void CreateInvTransaction(int ProdId, int WhId, double Qty, string TransType)
        {
            InvTransaction InvTrans = new InvTransaction
            {
                ProdId = ProdId,
                WhId = WhId,
                Qty = Qty,
                TransferType = TransType,
                CreatedById = GetLoggedInUserId(),
                CreatedDateTime = DateTime.Now
            };
            _db.InvTransaction.Add(InvTrans);
        }

        private bool CheckQtyInWh(int ProdId, int WhId, double Qty)
        {
            InvStockQty = _db.InvStockQty.FirstOrDefaultAsync(inv => inv.ProdId == ProdId & inv.WhId == WhId).GetAwaiter().GetResult();

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
