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

namespace SumerBusinessSolution.Pages.Sales.Billings
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ISalesTrans _SalesTrans;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public EditModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }
        [BindProperty]
        public BillHeader BillHeader { get; set; }

        [BindProperty]
        public BillItems BillItems { get; set; }
        [BindProperty]


        public List<BillItems> BillItemsList { get; set; }

        [BindProperty]
        public CompanyInfo CompanyInfo { get; set; }

        [BindProperty]
        [Display(Name = "مبلغ دفع الجديد")]
        public double NewPaidAmt { get; set; }

        [BindProperty]
        [Display(Name = "التخفيض الجديد")]
        public double NewDiscount { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<ActionResult> OnGet(int BhId)
        {

            BillItemsList = await _db.BillItems
                .Include(bill => bill.BillHeader)
                .Include(bill => bill.BillHeader.Customer)
                .Include(bill => bill.ProdInfo)
                .Include(bill => bill.BillHeader.ApplicationUser)
                .Where(bill => bill.HeaderId == BhId).ToListAsync();
            if (BillItemsList.Count() > 0)
            {
                BillHeader = BillItemsList[0].BillHeader;
            }

            try
            {
                CompanyInfo = _db.CompanyInfo.FirstOrDefault();
            }
            catch
            {

            }

            return Page();
        }
        public ActionResult OnPostEditBill(int HeaderId)
        {
            
            StatusMessage = _SalesTrans.EditBill(HeaderId, NewPaidAmt, NewDiscount).GetAwaiter().GetResult();
            return RedirectToPage("/Sales/Billings/Details", new { BhId = BillHeader.Id });
            // edit bill header
        }

    }
}