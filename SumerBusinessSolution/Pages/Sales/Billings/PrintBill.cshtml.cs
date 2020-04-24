using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution
{
    public class PrintBillModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public PrintBillModel(ApplicationDbContext db)
        {
            _db = db;
 
        }
        [BindProperty]
        public BillHeader BillHeader { get; set; }

        [BindProperty]
        public BillItems BillItems { get; set; }

        public List<BillItems> BillItemsList { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<ActionResult> OnGet(int BhId)
        {

            BillItemsList = await _db.BillItems
                .Include(bill => bill.BillHeader).Include(bill => bill.BillHeader.Customer).Include(bill => bill.ProdInfo).Include(bill => bill.BillHeader.ApplicationUser).Where(bill => bill.HeaderId == BhId).ToListAsync();
            if (BillItemsList.Count() > 0)
            {
                BillHeader = BillItemsList[0].BillHeader;
            }
            return Page();
        }
        public void OnPost()
        {

        }
    }
}
 