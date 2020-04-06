using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Customers.Customers
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class EditModel : PageModel
    {
            private readonly ApplicationDbContext _db;
        

            public EditModel(ApplicationDbContext db)
            {
                _db = db;
            }
        [BindProperty]
        public Customer Customer { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGet(int? CustId)
            {
  
                Customer = await _db.Customer.FirstOrDefaultAsync(cus => cus.Id == CustId);
                return Page();

            }

            public async Task<IActionResult> OnPost()
            {
              
                var CustFromDB = await _db.Customer.FirstOrDefaultAsync(cus => cus.Id == Customer.Id);
                
                if (CustFromDB == null)
                {
                    return NotFound();
                }

            CustFromDB.CompanyName = Customer.CompanyName;
            CustFromDB.ContactName = Customer.ContactName;
            CustFromDB.Address = Customer.Address;
            CustFromDB.PhoneNo = Customer.PhoneNo;

                await _db.SaveChangesAsync();
            StatusMessage = "تم التعديل على معلومات الزبون";

            return RedirectToPage("/Customers/Customers/Edit", new { CustId = CustFromDB.Id });
        }
        }
    }
 