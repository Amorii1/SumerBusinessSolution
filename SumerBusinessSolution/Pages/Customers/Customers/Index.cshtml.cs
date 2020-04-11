using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Customers.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }


        [BindProperty]
        public  Customer  Customer  { get; set; }

        [BindProperty]
        public  List<Customer> CustomerList { get; set; }

     
        public async Task<IActionResult> OnGet(string CustomerName = null)
        {
            StringBuilder Param = new StringBuilder();
            if (CustomerName != null)
            {
                Param.Append(CustomerName);
            }
            Param.Append("&CustomerName=");
            if (CustomerName != null)
            {
                CustomerList = await _db.Customer
                    .Where(cus => cus.Status == SD.ActiveCustomer & cus.CompanyName == CustomerName).ToListAsync();

            }
            else
            {
                CustomerList = await _db.Customer.Where(cus => cus.Status == SD.ActiveCustomer).ToListAsync();

            }
            return Page();
        }

        public JsonResult OnGetSearchCustomer(string term)
        {
            if (term == null)
            {
                return new JsonResult("Not Found");
            }
            IQueryable<string> lstCustomers = from P in _db.Customer
                                              where (P.CompanyName.Contains(term))
                                              select P.CompanyName;
            return new JsonResult(lstCustomers);
        }
    }
}