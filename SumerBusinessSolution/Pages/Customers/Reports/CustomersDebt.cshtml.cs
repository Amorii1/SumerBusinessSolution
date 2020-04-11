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

namespace SumerBusinessSolution
{
    public class CustomersDebtModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        public CustomersDebtModel(ApplicationDbContext db)
        {
            _db = db;
        }


        [BindProperty]
        public Customer Customer { get; set; }

        [BindProperty]
        public CustAcc CustAcc { get; set; }

        [BindProperty]
        public List<CustAcc> CustAccList { get; set; }

        public async Task<IActionResult> OnGet(string CustomerName = null)
        {

            StringBuilder Param = new StringBuilder();

            Param.Append("&SearchCustomer=");

            if (CustomerName != null)
            {
                Param.Append(CustomerName);
            }
            Param.Append("&CustomerName=");

            if (CustomerName != null)
            {
                CustAccList = await _db.CustAcc.Include(cus => cus.Customer).Where(cus => cus.Customer.CompanyName.ToLower().Contains(CustomerName.ToLower()) & cus.Debt > 0).ToListAsync();
            }
            else
            {
                CustAccList = await _db.CustAcc.Include(cus => cus.Customer).Where(cus => cus.Customer.Status == SD.ActiveCustomer & cus.Debt > 0).ToListAsync();
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
 