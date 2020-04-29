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
    public class CreateModel : PageModel
    {

        private readonly ApplicationDbContext _db;
 
 
        //private readonly IServiceScopeFactory _serviceScopeFactory;

        [BindProperty]
        public CompanyInfo CompanyInfo { get; set; }
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
 
 
        }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            _db.CompanyInfo.Add(CompanyInfo); // like migration 
            await _db.SaveChangesAsync();

           

            return RedirectToPage("Index");
        }

    }
}