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
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _db;
  

        //private readonly IServiceScopeFactory _serviceScopeFactory;

        [BindProperty]
        public CompanyInfo CompanyInfo { get; set; }
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
    

        }
        public async Task<IActionResult> OnGet()
        {
            CompanyInfo = await _db.CompanyInfo.FirstOrDefaultAsync(ci=> ci.Id == 1);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {

                CompanyInfo CompanyInfoFromDB = await _db.CompanyInfo.FirstOrDefaultAsync(ci => ci.Id == 1);


                if (!ModelState.IsValid)
                {
                    return Page();
                }

                CompanyInfoFromDB.CompanyNameEn = CompanyInfo.CompanyNameEn;
                CompanyInfoFromDB.CompanyNameAr = CompanyInfo.CompanyNameAr;
                CompanyInfoFromDB.AddressEn = CompanyInfo.AddressEn;
                CompanyInfoFromDB.AddressAr = CompanyInfo.AddressAr;
                CompanyInfoFromDB.Email = CompanyInfo.Email;
                CompanyInfoFromDB.PhoneNo = CompanyInfo.PhoneNo;
                CompanyInfoFromDB.PhoneNo02 = CompanyInfo.PhoneNo02;
                CompanyInfoFromDB.Note = CompanyInfo.Note;

                await _db.SaveChangesAsync();
            }
            catch
            {

            }

           // _reqNote.SendMessage("Ahmed", "New request for you!!").GetAwaiter().GetResult();

            return RedirectToPage("Index");
        }



    }
}