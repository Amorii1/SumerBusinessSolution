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
using Microsoft.AspNetCore.Localization;
using System.ComponentModel.DataAnnotations;


namespace SumerBusinessSolution
{ 
    public class DetailsoldModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        private readonly ISalesTrans _SalesTrans;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public DetailsoldModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }
        [BindProperty]
        public BillHeader BillHeader { get; set; }

        [BindProperty]
        public BillItems BillItems { get; set; }

        public List<BillItems> BillItemsList { get; set; } 

        //[BindProperty]
        //public string CompanyName { get; set; }

        //[BindProperty]
        //public double PaidAmt { get; set; }


        //[BindProperty]
        //public int HeaderId { get; set; }

        public async Task<ActionResult> OnGet(int BhId)
        {
            //BillHeader = await _db.BillHeader.Include(h => h.Customer)
            //   .FirstOrDefaultAsync(h => h.Id == BhId);



            BillItemsList = await _db.BillItems.Include(bill=> bill.BillHeader).Include(bill=> bill.BillHeader.Customer).Include(bill=> bill.ProdInfo).Include(bill=> bill.BillHeader.ApplicationUser).Where(bill => bill.HeaderId == BhId).ToListAsync();
            if(BillItemsList.Count() > 0)
            {
                BillHeader = BillItemsList[0].BillHeader;
            }



            return Page();
        }
        public void OnPost()
        {
            //StatusMessage = _SalesTrans.MakePaymentOnBill(HeaderId, NewPayment).GetAwaiter().GetResult();

           // return RedirectToPage("Index");
        }
    }
}