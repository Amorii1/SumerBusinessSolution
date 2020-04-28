using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Utility;

namespace SumerBusinessSolution
{
    [Authorize]
    public class PrintExternalBillModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public PrintExternalBillModel(ApplicationDbContext db)
        {
            _db = db;

        }
        [BindProperty]
        public ExternalBillHeader ExternalBillHeader { get; set; }

        [BindProperty]
        public ExternalBillItems ExternalBillItems { get; set; }

        // THE COMPANY INFO CODE BELOW

        public CompanyInfo CompanyInfo { get; }
        public List<ExternalBillItems> ExternalItemsList { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<ActionResult> OnGet(int BhId)
        {

            ExternalItemsList = await _db.ExternalBillItems
                .Include(bill => bill.ExternalBillHeader)
                .Include(bill => bill.ExternalBillHeader.Customer)
                .Include(bill => bill.ProdInfo)
                .Include(bill => bill.ExternalBillHeader.ApplicationUser)
                .Where(bill => bill.HeaderId == BhId).ToListAsync();
            if (ExternalItemsList.Count() > 0)
            {
                ExternalBillHeader = ExternalItemsList[0].ExternalBillHeader;
            }
            return Page();
        }
        public void OnPost()
        {

        }
    }
}
