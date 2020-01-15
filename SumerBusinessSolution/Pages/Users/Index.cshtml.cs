using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Models.ViewModels;

namespace SumerBusinessSolution.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private object SearchName;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IList<ApplicationUser> ApplicationUser { get; set; }

        [BindProperty]
        public UsersListViewModel UsersListVM { get; set; }

        public async Task<IActionResult> OnGet(string searchFirstName = null)
        {

            UsersListVM = new UsersListViewModel()
            {
                ApplicationUserList = await _db.ApplicationUser.ToListAsync()
            };

            StringBuilder Param = new StringBuilder();

            Param.Append("&searchFirstName=");

            if (searchFirstName != null)
            {
                Param.Append(searchFirstName);
            }
            if (searchFirstName != null)
            {
                UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.FirstName.ToLower().Contains(searchFirstName.ToLower())).ToListAsync();

            }

            return Page();
        }
    }
}