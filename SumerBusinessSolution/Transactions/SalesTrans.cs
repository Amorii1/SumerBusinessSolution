using Microsoft.AspNetCore.Http;
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

        // this function will create a bill
        public async Task<string> CreateBill(BillHeader Header, List<BillItems> BillItems)
        {
            try
            {
                Header.CreatedById = GetLoggedInUserId();
                Header.CraetedDataTime = DateTime.Now;

                double TotalAmt = 0;

                // getting the unit price of each item in the bill items 
                foreach (BillItems item in BillItems)
                {
                    TotalAmt += item.UnitPrice;
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


        // Helping functions // 
        private string GetLoggedInUserId()
        {
            var ClaimId = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var Claim = ClaimId.FindFirst(ClaimTypes.NameIdentifier);
            string UserId = Claim.Value;
            return UserId;
        }
    }
}
