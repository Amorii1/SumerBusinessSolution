using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Pages.Customers.Accounts
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class PayToAccModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ISalesTrans _SalesTrans;

        public PayToAccModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }

        //[BindProperty]
        //public Customer Customer { get; set; }

        [BindProperty]
        public CustAcc CustAcc { get; set; }

        [BindProperty]
        [Display(Name = "دفع مبلغ")]
        public double NewPayment { get; set; }

        [BindProperty]
        public int CustomerId { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet(int CustId)
        {
           
            CustAcc = await _db.CustAcc.Include(acc=> acc.Customer).FirstOrDefaultAsync(acc => acc.CustId == CustId);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {

            StatusMessage = _SalesTrans.MakePaymentToAcc(CustAcc.Customer.Id, NewPayment).GetAwaiter().GetResult();

            return RedirectToPage("/Customers/Accounts/PayToAcc",new { CustId = CustAcc.Customer.Id });
        }
    }
}
 