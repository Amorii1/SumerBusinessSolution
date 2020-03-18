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
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class EditCustAccModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        private readonly ISalesTrans _SalesTrans;

        public EditCustAccModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }
        [BindProperty]
        public Customer Customer { get; set; }

        [BindProperty]
        public CustAcc CustAcc { get; set; }

        [BindProperty]
        public double NewPayment { get; set; }

        [BindProperty]
        public double NewDebt { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet(int CustId)
        {
      
            CustAcc = await _db.CustAcc.Include(acc=> acc.Customer).FirstOrDefaultAsync(acc => acc.CustId == CustId);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            StatusMessage = _SalesTrans.UpdateCustomerAccManually(CustAcc.Customer.Id, NewPayment, NewDebt).GetAwaiter().GetResult();
            return RedirectToPage("/Customers/Accounts/EditCustAcc", new { CustId = CustAcc.Customer.Id });
        }

    }
}
 