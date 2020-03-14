using Microsoft.AspNetCore.Http;
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
                    TotalAmt += item.ProdInfo.RetailPrice;

                }

                // price before discount
                Header.TotalAmt = TotalAmt;

                // incase there is a discount
                TotalAmt = TotalAmt - Header.Discount;
                Header.TotalNetAmt = TotalAmt;

                _db.BillHeader.Add(Header);
                await _db.SaveChangesAsync();

                // Creating Bill items
                foreach (BillItems item in BillItems)
                {
                    item.HeaderId = Header.Id;
                    item.UnitPrice = item.ProdInfo.RetailPrice;
                    item.TotalAmt = item.ProdInfo.RetailPrice * item.Qty;

                    _db.BillItems.Add(item);
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
