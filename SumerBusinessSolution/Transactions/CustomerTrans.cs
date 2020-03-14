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
    public class CustomerTrans : ICustomerTrans
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerTrans(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;

        }

        public Customer Customer { get; set; }

        // This function will create a new customer and a new record of Customer Account
        public async Task<string> CreateCustomer(Customer NewCust)
        {
            try
            {
                NewCust.Status = SD.ActiveCustomer;
                NewCust.CreatedById = GetLoggedInUserId();
                NewCust.CreatedDateTime = DateTime.Now;

                _db.Customer.Add(NewCust);
                await _db.SaveChangesAsync();

                CreateCustomerAccount(NewCust.Id);

                return "تم اضافة زبون جديد";
            }
            catch
            {
                return "لم تتم علمية الاضافة";
            }
    
        }
        // this function Deactivates a customer
        public async Task<string> CreateCustomer(int CustId)
        {
              Customer = await _db.Customer.FirstOrDefaultAsync(cu => cu.Id == CustId);

            Customer.Status = SD.InactiveCustomer;

            await _db.SaveChangesAsync();

            return "تمت العملية بنجاح";
        }

        // Helper functions //

        private void CreateCustomerAccount(int CustId)
        {
            CustAcc NewAcc = new CustAcc
            {
                CustId = CustId,
                Paid = 0,
                Debt = 0
            };

            _db.CustAcc.Add(NewAcc);
            _db.SaveChangesAsync().GetAwaiter().GetResult();
 
        }

        private string GetLoggedInUserId()
        {
 
            var ClaimId = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var Claim = ClaimId.FindFirst(ClaimTypes.NameIdentifier);
            string UserId = Claim.Value;
            return UserId;
        }

    }
}
