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
    public class CreateWarehouseModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CreateWarehouseModel(ApplicationDbContext db)
        {
            _db = db;

        }
        [BindProperty]
        public Warehouse Warehouse { get; set; }
        public WhType WhType { get; set; }
        public int TypeId { get; set; }

        public async Task<IActionResult> OnGet()
        {
            WhType = await _db.WhType.FirstOrDefaultAsync(t => t.Id == TypeId);
            return Page();
        }
       
        public async Task<IActionResult> OnPost()
        {

            _db.Warehouse.Add(Warehouse);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}