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
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Inventory.Warehouses
{
    [Authorize]
    [Authorize(Roles = SD.AdminEndUser)]
    public class Create : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Create(ApplicationDbContext db)
        {
            _db = db;

        }
        [BindProperty]
        public Warehouse Warehouse { get; set; }
        [Required]
        public WhType WhType { get; set; }
        public int TypeId { get; set; }

        public async Task<IActionResult> OnGet()
        {

           

            WhType = await _db.WhType.FirstOrDefaultAsync(t => t.Id == TypeId);
            return Page();
        }
       
        public async Task<IActionResult> OnPost()
        {
            var ClaimId = (ClaimsIdentity)User.Identity;
            var Claim = ClaimId.FindFirst(ClaimTypes.NameIdentifier);
            string UserId = Claim.Value;

            Warehouse.CreatedById = UserId;
          //  Warehouse.CreatedById = Warehouse.ApplicationUser.Id;
            _db.Warehouse.Add(Warehouse);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}