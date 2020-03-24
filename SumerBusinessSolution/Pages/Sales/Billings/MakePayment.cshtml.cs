using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;
using Microsoft.AspNetCore.Localization;


namespace SumerBusinessSolution.Pages.Sales.Billings
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
        [Display(Name = "اسم الشركة")]
        public string CompanyName { get; set; }

        [BindProperty]
        [Display(Name = "المبلغ المدفوع")]
        public double PaidAmt { get; set; }

        [BindProperty]
        [Display(Name = "المبلغ المتبقي")]
        public double TotalNetAmt { get; set; }

        [BindProperty]
        [Display(Name = "دفع مبلغ")]
        public double NewPayment { get; set; }

        [BindProperty]
        [Display(Name = "رقم الفاتورة")]
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
        public async Task<ActionResult> OnPost(int HeaderId)
        {
            StatusMessage = _SalesTrans.MakePaymentOnBill(HeaderId, NewPayment).GetAwaiter().GetResult();

            return RedirectToPage("Index");
        }
    }
}