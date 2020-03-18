using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Customers.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }


        [BindProperty]
        public  Customer  Customer  { get; set; }

        [BindProperty]
        public  List<Customer> CustomerList { get; set; }


        public async Task<IActionResult> OnGet()
        {
            CustomerList = await _db.Customer.Where(cus => cus.Status == SD.ActiveCustomer).ToListAsync();
            return Page();
        }
    }
}