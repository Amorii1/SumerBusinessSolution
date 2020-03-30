using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Inventory.WhTypes
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class Create : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Create(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public WhType WhType { get; set; }
        public async Task<IActionResult> OnGet(int TypeId)
        {
            WhType = await _db.WhType.FirstOrDefaultAsync(t => t.Id == TypeId);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _db.WhType.Add(WhType);
            await _db.SaveChangesAsync();
            return Page();
        }
    }
}