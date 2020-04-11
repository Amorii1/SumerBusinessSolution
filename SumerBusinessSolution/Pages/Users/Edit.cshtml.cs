using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Users
{
    [Authorize(Roles = SD.AdminEndUser)]

  //[Authorize(Roles = SD.SupervisorEndUser)]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public EditModel(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        [Display(Name = "صفة المستخدم")]
        public string SelectedRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id.Trim().Length == 0)
            {
                return NotFound();
            }

            ApplicationUser = await _db.ApplicationUser.FirstOrDefaultAsync(m => m.Id == id);
            //var user = await _userManager.FindByIdAsync(id);
            //var roles = _db.Roles.FirstOrDefault(r => r.) //await _userManager.GetRolesAsync(user);
            //string userRole = roles.Select(r=> r)
            SelectedRole = SD.AdminEndUser;

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            //else
            //{
                var userInDb = await _db.ApplicationUser.SingleOrDefaultAsync(u => u.Id == ApplicationUser.Id);
                if (userInDb == null)
                {
                    return NotFound();
                }
                else
                {
                    userInDb.FirstName = ApplicationUser.FirstName;
                    userInDb.LastName = ApplicationUser.LastName;

                    userInDb.PhoneNumber = ApplicationUser.PhoneNumber;
                   // userInDb.UserName = ApplicationUser.UserName;
                 //   userInDb.Email = ApplicationUser.Email;
                    
                    await _db.SaveChangesAsync();
                    return RedirectToPage("Index");
                }
           // }
        }

    }
}