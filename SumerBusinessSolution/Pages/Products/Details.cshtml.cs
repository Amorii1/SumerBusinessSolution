using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public ProdInfo ProdInfo { get; set; }
        public async Task<IActionResult> OnGet(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            ProdInfo = await _db.ProdInfo.FirstOrDefaultAsync(m => m.Id == ID);
            if (ProdInfo == null)
            {

                return NotFound();
            }
            return Page();
        }
    }
}