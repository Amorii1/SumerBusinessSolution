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

namespace SumerBusinessSolution
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class CreateModel : PageModel
    {
            private readonly ApplicationDbContext _db;
        private readonly ICustomerTrans _CustTrans;

            public CreateModel(ApplicationDbContext db, ICustomerTrans CustTrans)
            {
                _db = db;
            _CustTrans = CustTrans;
            }

        [BindProperty]

            public Customer Customer { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public string CompanyName { get; set; }

        [BindProperty]
        public string ContactName { get; set; }

        [BindProperty]
        public string Address { get; set; }

        [BindProperty]
        public string PhoneNo { get; set; }


        public IActionResult OnGet()
            {
                return Page();
            }

            public async Task<IActionResult> OnPostAsync()
            {
                //if (!ModelState.IsValid)
                //{
                //    return Page();
                //}

                StatusMessage = _CustTrans.CreateCustomer(Customer).GetAwaiter().GetResult() ;

              
                return Page();  
            }
        }
    }
 