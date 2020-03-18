using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution
{
    public class CustAccDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CustAccDetailsModel(ApplicationDbContext db )
        {
            _db = db;
        }

        [BindProperty]

        public Customer Customer { get; set; }

        [BindProperty]
        public CustAcc CustAcc { get; set; }

        public async Task<IActionResult> OnGet(int CustId)
        {
            Customer = await _db.Customer.FirstOrDefaultAsync(cus => cus.Id == CustId);
            CustAcc = await _db.CustAcc.FirstOrDefaultAsync(acc => acc.CustId == CustId);
            return Page();
        }
    }
}