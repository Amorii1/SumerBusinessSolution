using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Products
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _host;

        public EditModel(ApplicationDbContext db, IHostingEnvironment host)
        {
            _db = db;
            _host = host;
        }
        [BindProperty]
        public ProdInfo ProdInfo { get; set; }
        [BindProperty]
        public IFormFile img { get; set; }

        public async Task<IActionResult> OnGet(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            ProdInfo = await _db.ProdInfo.FirstOrDefaultAsync(m => m.Id == ID);
            if (ProdInfo == null)
            {

                return NotFound();
            }
            return Page();

        }

        public async Task<IActionResult> OnPost()
        {
            string newfileName = string.Empty;
            if (img != null && img.Length > 0)
            {

                string fn = img.FileName;
                if (IsImagValidate(fn))
                {
                    string extension = Path.GetExtension(fn);
                    newfileName = Guid.NewGuid().ToString() + extension;
                    string filename = Path.Combine(_host.WebRootPath + "/Img/", newfileName);
                    await img.CopyToAsync(new FileStream(filename, FileMode.Create));
                }
                else
                {
                    return Page();
                }
            }
            var ProductFromDB = await _db.ProdInfo.FirstOrDefaultAsync(m => m.Id == ProdInfo.Id);
            if (ProductFromDB == null)
            {
                return NotFound();
            }
            ProductFromDB.ProdCode = ProdInfo.ProdCode;
            ProductFromDB.ProdName = ProdInfo.ProdName;
            ProductFromDB.ProdCategory = ProdInfo.ProdCategory;
            ProductFromDB.ProdDescription = ProdInfo.ProdDescription;
            ProductFromDB.CreatedById = ProdInfo.CreatedById;
            ProductFromDB.CreatedDateTime = ProdInfo.CreatedDateTime;
            ProductFromDB.ImgFile = ProdInfo.ImgFile;
            ProductFromDB.CostPrice = ProdInfo.CostPrice;
            ProductFromDB.RetailPrice = ProdInfo.RetailPrice;
            ProductFromDB.WholePrice = ProdInfo.WholePrice;
 

            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
        private bool IsImagValidate(string filename)
        {
            string extension = Path.GetExtension(filename);
            if (extension.Contains(".png"))
                return true;
            if (extension.Contains(".PNG"))
                return true;
            if (extension.Contains(".jpeg"))
                return true;
            if (extension.Contains(".jpg"))
                return true;
            if (extension.Contains(".gif"))
                return true;
            return false;
        }

    }
}