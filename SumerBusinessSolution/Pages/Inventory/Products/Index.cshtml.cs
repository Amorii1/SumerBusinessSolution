using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Models.ViewModels;

namespace SumerBusinessSolution.Pages.Inventory.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        private object SearchProduct;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IList<ProdInfo> ProdInfo { get; set; }
        public ProdListViewModel ProductListVM { get; set; }

        public async Task<IActionResult> OnGet(string SearchProdCode = null, string SearchProdName = null)
        {
            ProdInfo = await _db.ProdInfo.ToListAsync();
            ProductListVM = new ProdListViewModel()
            {
                ProdInfoList = await _db.ProdInfo.ToListAsync()
            };

            StringBuilder Param = new StringBuilder();

            Param.Append("&SearchProdCode=");
            Param.Append("&SearchProdName=");

            if (SearchProdCode != null)
            {
                Param.Append(SearchProdCode);
            }
            if (SearchProdName != null)
            {
                Param.Append(SearchProdName);
            }
            if (SearchProdCode != null)
            {
                 ProdInfo = await _db.ProdInfo.Where(u => u.ProdCode.ToLower().Contains(SearchProdCode.ToLower())).ToListAsync();
            }
            else
            {
                if (SearchProdName != null)
                {
                    ProdInfo = await _db.ProdInfo.Where(u => u.ProdName.ToLower().Contains(SearchProdName.ToLower())).ToListAsync();
                }
            }
           

            return Page();
        }
        public JsonResult OnGetSearchNow(string term)
        {
            if (term == null)
            {
                return new JsonResult("Not Found");
            }
            IQueryable<string> lstProdCode = from P in _db.ProdInfo
                                             where (P.ProdCode.Contains(term))
                                             select P.ProdCode;
            return new JsonResult(lstProdCode);

        }

        public JsonResult OnGetSearchNameNow(string term)
        {
            if (term == null)
            {
                return new JsonResult("Not Found");
            }
            IQueryable<string> lstProdName = from P in _db.ProdInfo
                                             where (P.ProdName.Contains(term))
                                             select P.ProdName;
            return new JsonResult(lstProdName);

        }


        //public async Task<IActionResult> OnGet()
        //{

        //    ProductInfo = await _db.ProductInfo.ToListAsync();
        //    return Page();

        //}
    }
}
