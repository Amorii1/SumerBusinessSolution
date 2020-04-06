using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;
using Microsoft.AspNetCore.Localization;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SumerBusinessSolution.Pages.Sales.ExternalBillings
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
            public ExternalBillHeader ExternalBillHeader { get; set; }

            [BindProperty]
            public IEnumerable<ExternalBillHeader> ExternalBillHeaderList { get; set; }
            public List<Customer> CustomerList { get; set; }


            [BindProperty]
            public int HeaderId { get; set; }

            [TempData]
            public string StatusMessage { get; set; }

        [TempData]
        public bool ShowAll { get; set; }


        public  IActionResult OnGet(string CustomerName = null, bool ShowAll = false)
        {
            StringBuilder Param = new StringBuilder();
            Param.Append("&SearchCustomer=");

            if (CustomerName != null)
            {
                Param.Append(CustomerName);
            }

            Param.Append("&CustomerName=");


            if (ShowAll == true)
            {
                Param.Append(ShowAll);
            }

            Param.Append("&ShowAll=");

            CustomerList = _db.Customer.ToList();

            if (CustomerName != null)
            {
                if (ShowAll == true)
                {
                    ExternalBillHeaderList = _db.ExternalBillHeader
                        .Where(header => header.Customer.CompanyName.ToLower().Contains(CustomerName.ToLower())).ToList()
                        .OrderByDescending(header => header.CreatedDataTime);
                }
                else
                {

                    ExternalBillHeaderList = _db.ExternalBillHeader
                        .Where(header => header.Status == SD.OpenBill & header.Customer.CompanyName.ToLower().Contains(CustomerName.ToLower())).ToList()
                        .OrderByDescending(header => header.CreatedDataTime);
                }
            }
            else
            {
                if (ShowAll == true)
                {
                    ExternalBillHeaderList = _db.ExternalBillHeader.ToList()
                        .OrderByDescending(header => header.CreatedDataTime);

                }
                else
                {
                    ExternalBillHeaderList = _db.ExternalBillHeader.Where(header => header.Status == SD.OpenBill).ToList()
                        .OrderByDescending(header => header.CreatedDataTime);

                }
            }
                return Page();
        }

        public IActionResult OnPostCloseBillManually(int ExternalHeaderId)
        {

            StatusMessage = _SalesTrans.CloseExternalBillManually(ExternalHeaderId).GetAwaiter().GetResult();

            return RedirectToPage("/Sales/ExternalBillings/Index", new { CustomerName = "" });
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


 