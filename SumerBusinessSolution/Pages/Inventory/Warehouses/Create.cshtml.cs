using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Inventory.Warehouses
{
    [Authorize]
    [Authorize(Roles = SD.AdminUser)]
    public class Create : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Create(ApplicationDbContext db)
        {
            _db = db;

        }
        [BindProperty]
        public Warehouse Warehouse { get; set; }
        [BindProperty]
        public List<WhType> WhTypeList { get; set; }
        [Required]
        public WhType WhType { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            WhTypeList = await _db.WhType.ToListAsync();
            return Page();
        }
       
        public async Task<IActionResult> OnPost()
        {
            var ClaimId = (ClaimsIdentity)User.Identity;
            var Claim = ClaimId.FindFirst(ClaimTypes.NameIdentifier);
            string UserId = Claim.Value;

            Warehouse.CreatedById = UserId;
            Warehouse.CreatedDateTime = DateTime.Now;
            Warehouse.Active = true;
            _db.Warehouse.Add(Warehouse);
            await _db.SaveChangesAsync();

            StatusMessage = "تمت اضافة مخزن جديد";

            return RedirectToPage("/Inventory/Warehouses/Create");
        }
    }
}