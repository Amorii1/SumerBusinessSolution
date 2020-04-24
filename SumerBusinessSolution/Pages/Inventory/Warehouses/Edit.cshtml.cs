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

namespace SumerBusinessSolution.Pages.Inventory.Warehouses
{
    [Authorize(Roles = SD.AdminUser)]
    public class EditModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Warehouse Warehouse { get; set; }

        [BindProperty]
        public List<WhType> WhTypeList { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet(int? WhId)
        {
            if (WhId == null)
            {
                return NotFound();
            }

            Warehouse = await _db.Warehouse.FirstOrDefaultAsync(m => m.Id == WhId);
            WhTypeList = await _db.WhType.ToListAsync();
            return Page();
        }


        public async Task<IActionResult> OnPost()
        {
            var WhFromDB = await _db.Warehouse.FirstOrDefaultAsync(m => m.Id == Warehouse.Id);
            if (WhFromDB == null)
            {
                return NotFound();
            }

            WhFromDB.WhCode = Warehouse.WhCode;
            WhFromDB.WhName = Warehouse.WhName;
            WhFromDB.TypeId = Warehouse.TypeId;
            WhFromDB.WhLocation = Warehouse.WhLocation;
            WhFromDB.Active = Warehouse.Active;
            await _db.SaveChangesAsync();
            StatusMessage = "لقد تمت عملية التعديل";
            return RedirectToPage("Index");
        }
    }
}