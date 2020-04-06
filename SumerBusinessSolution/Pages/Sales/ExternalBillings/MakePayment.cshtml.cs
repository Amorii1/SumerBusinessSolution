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


namespace SumerBusinessSolution.Pages.Sales.ExternalBillings
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
        public ExternalBillHeader ExternalBillHeader { get; set; }

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
        public int ExternalHeaderId { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<ActionResult> OnGet(int BhId)
        {
            ExternalBillHeader ExternalBillHeader = await _db.ExternalBillHeader.Include(h=> h.Customer)
                .FirstOrDefaultAsync(h => h.Id == BhId);
            ExternalHeaderId = ExternalBillHeader.Id;
            CompanyName = ExternalBillHeader.Customer.CompanyName;
            TotalNetAmt = ExternalBillHeader.TotalNetAmt;
            PaidAmt = ExternalBillHeader.PaidAmt;
            NewPayment = 0;


            return Page();
        }
        public async Task<ActionResult> OnPost(int ExternalHeaderId)
        {
            StatusMessage = _SalesTrans.MakePaymentOnExternalBill(ExternalHeaderId, NewPayment).GetAwaiter().GetResult();

            return RedirectToPage("MakePayment", new { BhId = ExternalHeaderId });
        }
    }
}