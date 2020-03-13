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
    public class CustomerTrans : ICustomerTrans
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerTrans(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;

        }

        // This function will create a new customer and a new record of Customer Account
        public async Task<string> CreateCustomer(string ComName, string ContactName, string Address, string PhoneNo) 
        {
            try
            {
                Customer NewCust = new Customer
                {
                    CompanyName = ComName,
                    ContactName = ContactName,
                    Address = Address,
                    PhoneNo = PhoneNo,
                    Status = SD.ActiveCustomer,
                    CreatedBy = GetLoggedInUserId(),
                    CreatedDateTime = DateTime.Now
                };

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
