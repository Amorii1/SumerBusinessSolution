using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;
using Microsoft.AspNetCore.Localization;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SumerBusinessSolution.Pages.Sales.Billings
{
    [Authorize]
    public class IndexModel : PageModel
    {

         private readonly ApplicationDbContext _db;
         private readonly ISalesTrans _SalesTrans;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public IndexModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }
            public BillHeader BillHeader { get; set; }

            [BindProperty]
            public IEnumerable<BillHeader> BillHeaderList { get; set; }
            public List<Customer> CustomerList { get; set; }


            [BindProperty]
            public int HeaderId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "من")]
        public DateTime? SearchFromDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "الى")]
        public DateTime? SearchToDate { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public  IActionResult OnGet(string CustomerName = null, DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            StringBuilder Param = new StringBuilder();
            Param.Append("&SearchCustomer=");

            if (CustomerName != null)
            {
                Param.Append(CustomerName);
            }
            Param.Append("&CustomerName=");

                CustomerList = _db.Customer.ToList();

            if (SearchFromDate != null & SearchToDate != null & CustomerName == null)
            {
                BillHeaderList = _db.BillHeader
                    .Where(header => header.Status == SD.OpenBill & header.CreatedDataTime >= SearchFromDate & header.CreatedDataTime <= SearchToDate).ToList()
                    .OrderByDescending(header => header.CreatedDataTime);
             }
            else
            {
                if (SearchFromDate != null & SearchToDate != null & CustomerName != null)
                {
                    BillHeaderList =   _db.BillHeader
                        .Include(header => header.Customer)
                        .Where(header => header.Customer.CompanyName.ToLower().Contains(CustomerName.ToLower()) & header.Status == SD.OpenBill & header.CreatedDataTime >= SearchFromDate & header.CreatedDataTime <= SearchToDate).ToList()
                        .OrderByDescending(header => header.CreatedDataTime);
                 }
                else
                {
                    if (SearchFromDate == null & SearchToDate == null & CustomerName != null)
                    {
                        BillHeaderList = _db.BillHeader
                            .Include(header => header.Customer)
                            .Where(header => header.Customer.CompanyName.ToLower().Contains(CustomerName.ToLower()) & header.Status == SD.OpenBill).ToList()
                            .OrderByDescending(header => header.CreatedDataTime);
 
                    }
                    else
                    {
                        BillHeaderList = _db.BillHeader
                            .Include(header => header.Customer)
                            .Where(header => header.CreatedDataTime >= DateTime.Now.AddMonths(-1) & header.Status == SD.OpenBill).ToList()
                            .OrderByDescending(header => header.CreatedDataTime);
                     }
                }

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


 