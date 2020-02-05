using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Inventory.Transactions
{
    public class ProdTransModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public IList<InvTransaction> InvTransList { get; set; }

        public ProdTransModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGet(int ProdId, int WhId)
        {
            InvTransList = await _db.InvTransaction.Include(tr=> tr.ProdInfo).Include(tr=> tr.Warehouse).Include(tr=> tr.ApplicationUser)
                .Where(tr => tr.ProdId == ProdId & tr.WhId == WhId).ToListAsync();
            
            return Page();
        }
    }
}