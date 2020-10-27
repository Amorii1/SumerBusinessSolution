using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public ApplicationUser ApplicationUser { get; set; }
        public string userid { get; set; }
        public IFormFile imgf { get; set; }
        public IList<TempProdImg> AddPhoto { get; set; }

        [BindProperty]
        public double OpenQty { get; set; }

        [BindProperty]
        public List<Warehouse> WarehouseList { get; set; }

        [BindProperty]
        public int SelectedWarehouse { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet(int? ProdId)
        {
            ProdInfo =  _db.ProdInfo.Include(m => m.ApplicationUser).FirstOrDefault(m => m.Id == ProdId);

            WarehouseList = _db.Warehouse.Where(wh => wh.Active == true).ToList();
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
           // ProductFromDB.CreatedDateTime = ProdInfo.CreatedDateTime;
            // ProductFromDB.ImgFile = img.FileName.ToString(); // ProdInfo.ImgFile;
            ProductFromDB.CostPrice = ProdInfo.CostPrice;
            ProductFromDB.RetailPrice = ProdInfo.RetailPrice;
            ProductFromDB.WholePrice = ProdInfo.WholePrice;

            TempProdImg v = new TempProdImg
            {
                ImgFile = newfileName
            };
            if(v.ImgFile != null)
            {
                _db.TempProdImg.Add(v);
                await _db.SaveChangesAsync();
            }
          

            // ImgFile = newfileName;
            AddPhoto = await _db.TempProdImg.ToListAsync();


            AddPhoto = await _db.TempProdImg.ToListAsync();
            foreach (var item in AddPhoto)
            {
                var AddPhoto = new ProdImg
                {
                    ImgFile = item.ImgFile,
                    ProdId = ProdInfo.Id
                };
                _db.ProdImg.Add(AddPhoto);

                ProductFromDB.ImgFile = AddPhoto.ImgFile;
            }
           
            _db.TempProdImg.RemoveRange(AddPhoto);

            await _db.SaveChangesAsync();
            StatusMessage = "تم التعديل على المادة";
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

        //private readonly ApplicationDbContext _db;
        //private readonly IHostingEnvironment _host;

        //public EditModel(ApplicationDbContext db, IHostingEnvironment host)
        //{
        //    _db = db;
        //    _host = host;
        //}
        //[BindProperty]
        //public ProdInfo ProdInfo { get; set; }
        //[BindProperty]
        //public IFormFile img { get; set; }

        //[Display(Name = "صورة المنتج")]
        //public string ImgFile { get; set; }

        //public IFormFile imgf { get; set; }

        //public IList<TempProdImg> AddPhoto { get; set; }

        //public async Task<IActionResult> OnGet(int? ID)
        //{
        //    if (ID == null)
        //    {
        //        return NotFound();
        //    }
        //    ProdInfo = await _db.ProdInfo.FirstOrDefaultAsync(m => m.Id == ID);
        //    if (ProdInfo == null)
        //    {

        //        return NotFound();
        //    }
        //    return Page();

        //}

        //public async Task<IActionResult> OnPost()
        //{
        //    string newfileName = string.Empty;
        //    if (img != null && img.Length > 0)
        //    {

        //        string fn = img.FileName;
        //        if (IsImagValidate(fn))
        //        {
        //            string extension = Path.GetExtension(fn);
        //            newfileName = Guid.NewGuid().ToString() + extension;
        //            string filename = Path.Combine(_host.WebRootPath + "/Img/", newfileName);
        //            await img.CopyToAsync(new FileStream(filename, FileMode.Create));
        //        }
        //        else
        //        {
        //            return Page();
        //        }
        //    }
        //    var ProductFromDB = await _db.ProdInfo.FirstOrDefaultAsync(m => m.Id == ProdInfo.Id);
        //    if (ProductFromDB == null)
        //    {
        //        return NotFound();
        //    }
        //    ProductFromDB.ProdCode = ProdInfo.ProdCode;
        //    ProductFromDB.ProdName = ProdInfo.ProdName;
        //    ProductFromDB.ProdCategory = ProdInfo.ProdCategory;
        //    ProductFromDB.ProdDescription = ProdInfo.ProdDescription;
        //    ProductFromDB.CreatedById = ProdInfo.CreatedById;
        //    ProductFromDB.CreatedDateTime = ProdInfo.CreatedDateTime;
        //    // ProductFromDB.ImgFile = img.FileName.ToString(); // ProdInfo.ImgFile;
        //    ProductFromDB.CostPrice = ProdInfo.CostPrice;
        //    ProductFromDB.RetailPrice = ProdInfo.RetailPrice;
        //    ProductFromDB.WholePrice = ProdInfo.WholePrice;

        //    ImgFile = newfileName;
        //    AddPhoto = await _db.TempProdImg.ToListAsync();
        //    foreach (var item in AddPhoto)
        //    {
        //        var AddPhoto = new ProdImg
        //        {
        //            ImgFile = item.ImgFile,
        //            ProdId = ProductFromDB.Id
        //        };
        //        _db.ProdImg.Add(AddPhoto);

        //    }
        //    _db.TempProdImg.RemoveRange(AddPhoto);



        //    await _db.SaveChangesAsync();

        //    return RedirectToPage("Index");
        //}
        //private bool IsImagValidate(string filename)
        //{
        //    string extension = Path.GetExtension(filename);
        //    if (extension.Contains(".png"))
        //        return true;
        //    if (extension.Contains(".PNG"))
        //        return true;
        //    if (extension.Contains(".jpeg"))
        //        return true;
        //    if (extension.Contains(".jpg"))
        //        return true;
        //    if (extension.Contains(".gif"))
        //        return true;
        //    return false;
        //}

        //public async Task<IActionResult> OnPostAddNewPhoto()
        //{
        //    string newfileName = string.Empty;
        //    if (imgf != null && imgf.Length > 0)
        //    {

        //        string fn = imgf.FileName;
        //        if (IsImagValidate(fn))
        //        {
        //            string extension = Path.GetExtension(fn);
        //            newfileName = Guid.NewGuid().ToString() + extension;
        //            string filename = Path.Combine(_host.WebRootPath + "/Img/", newfileName);
        //            await imgf.CopyToAsync(new FileStream(filename, FileMode.Create));
        //        }
        //        else
        //        {
        //            return Page();
        //        }
        //    }
        //    TempProdImg v = new TempProdImg
        //    {
        //        ImgFile = newfileName
        //    };
        //    _db.TempProdImg.Add(v);
        //    await _db.SaveChangesAsync();
        //    return Page();
        //}

    }
}