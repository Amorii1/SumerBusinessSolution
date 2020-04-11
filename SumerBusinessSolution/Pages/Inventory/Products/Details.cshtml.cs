using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Inventory.Products
{
    [Authorize]

    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public IFormFile img { get; set; }
        public ProdInfo ProdInfo { get; set; }
        public async Task<IActionResult> OnGet(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            ProdInfo = await _db.ProdInfo.Include(m=> m.ApplicationUser).FirstOrDefaultAsync(m => m.Id == ID);
            if (ProdInfo == null)
            {

                return NotFound();
            }
            return Page();
        }
    }
}