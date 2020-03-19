using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution
{
    public class MakePaymentModel : PageModel 
    {

        private readonly ApplicationDbContext _db;
        private readonly ISalesTrans _SalesTrans;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public MakePaymentModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }
        public BillHeader BillHeader { get; set; }

        [BindProperty]
        public string CompanyName { get; set; }

        [BindProperty]
        public double PaidAmt { get; set; }

        [BindProperty]
        public double TotalNetAmt { get; set; }

        [BindProperty]
        public double NewPayment { get; set; }

        [BindProperty]
        public int HeaderId { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<ActionResult> OnGet(int BhId)
        {
            BillHeader BillHeader = await _db.BillHeader.Include(h=> h.Customer)
                .FirstOrDefaultAsync(h => h.Id == BhId);
            HeaderId = BillHeader.Id;
            CompanyName = BillHeader.Customer.CompanyName;
            TotalNetAmt = BillHeader.TotalNetAmt;
            PaidAmt = BillHeader.PaidAmt;
            NewPayment = 0;


            return Page();
        }
        public async Task<ActionResult> OnPost()
        {
            StatusMessage = _SalesTrans.MakePaymentOnBill(HeaderId, NewPayment).GetAwaiter().GetResult();

            return RedirectToPage("Index");
        }
    }
}