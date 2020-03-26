using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Inventory.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _host;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ApplicationDbContext db, IHostingEnvironment host, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _host = host;
            _userManager = userManager;

        }
        [BindProperty]
        public ProdInfo ProductInfo { get; set; }
        [BindProperty]
        public IFormFile img { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string userid { get; set; }
        public IFormFile imgf { get; set; }
        public IList<TempProdImg> AddPhoto { get; set; }

        //public async Task<IActionResult> OnGet()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var email = await _userManager.GetEmailAsync(user);
        //    ApplicationUser = _db.ApplicationUser.FirstOrDefault(c => c.Email == email);
        //    if (ApplicationUser==null)
        //    {
        //        return NotFound();
        //    }
        //    ProductInfo = new ProductInfo();
        //    ProductInfo.ApplicationUser.Id = ProductInfo.ID;
        //    ProductInfo.CreatedDateTime = DateTime.Now;
        //    return Page();
        //}
        public IActionResult OnGet(string AdminId = null)
        {
            if (AdminId == null)
            {
                var ClaimId = (ClaimsIdentity)User.Identity;
                var Claim = ClaimId.FindFirst(ClaimTypes.NameIdentifier);
                AdminId = Claim.Value;

            };

           // userid = AdminId;
            return Page();

        }


        public async Task<IActionResult> OnPostAddNewPhoto()
        {
            string newfileName = string.Empty;
            if (imgf != null && imgf.Length > 0)
            {

                string fn = imgf.FileName;
                if (IsImagValidate(fn))
                {
                    string extension = Path.GetExtension(fn);
                    newfileName = Guid.NewGuid().ToString() + extension;
                    string filename = Path.Combine(_host.WebRootPath + "/Img/", newfileName);
                    await imgf.CopyToAsync(new FileStream(filename, FileMode.Create));
                }
                else
                {
                    return Page();
                }
            }
            TempProdImg v = new TempProdImg
            {
                ImgFile = newfileName
            };
            _db.TempProdImg.Add(v);
            await _db.SaveChangesAsync();
            return Page();
        }


        public async Task<IActionResult> OnPost()
        {
            var ClaimId = (ClaimsIdentity)User.Identity;
            var Claim = ClaimId.FindFirst(ClaimTypes.NameIdentifier);
            string UserId = Claim.Value;

            if (!ModelState.IsValid)
            {
               // return Page();
            }
            string newfileName = string.Empty;
            if (img != null && img.Length > 0)
            {

                string fn = img.FileName;
                if (IsImagValidate(fn))
                {
                    string extension = Path.GetExtension(fn);
                    newfileName = Guid.NewGuid().ToString() + extension;
                    string filename = Path.Combine(_host.WebRootPath + "/img/", newfileName);
                    await img.CopyToAsync(new FileStream(filename, FileMode.Create));
                }
                else
                {
                    return Page();
                }
            }

            ProductInfo.CreatedDateTime = DateTime.Now;
            var Information = new ProdInfo
            {
                CreatedById = UserId,
                ProdCode = ProductInfo.ProdCode,
                ProdDescription = ProductInfo.ProdDescription,
                ProdName = ProductInfo.ProdName,
                ProdCategory = ProductInfo.ProdCategory,
                CostPrice = ProductInfo.CostPrice,
                RetailPrice = ProductInfo.RetailPrice,
                WholePrice = ProductInfo.WholePrice,
                CreatedDateTime = ProductInfo.CreatedDateTime,

                ImgFile = newfileName


            };

            AddPhoto = await _db.TempProdImg.ToListAsync();
            foreach (var item in AddPhoto)
            {
                var AddPhoto = new ProdImg
                {
                    ImgFile = item.ImgFile,
                    ProdId = Information.Id
                };
                _db.ProdImg.Add(AddPhoto);

            }
            _db.TempProdImg.RemoveRange(AddPhoto);
            _db.ProdInfo.Add(Information);
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