using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
 
namespace SumerBusinessSolution
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public WhType WhType { get; set; }

        [TempData]
        public string StatusMessage { get; set; }


        public async Task<IActionResult> OnGet(int? TypeId)
        {
            if (TypeId == null)
            {
                return NotFound();
            }
            WhType = await _db.WhType.FirstOrDefaultAsync(m => m.Id == TypeId);
            if (WhType == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var TypeFromDB = await _db.WhType.FirstOrDefaultAsync(m => m.Id == WhType.Id);
            if (TypeFromDB == null)
            {
                return NotFound();
            }
            TypeFromDB.Type = WhType.Type;

            await _db.SaveChangesAsync();
            StatusMessage = "لقد تمت عملية التعديل";
            return RedirectToPage("Index");
        }
    }     
}