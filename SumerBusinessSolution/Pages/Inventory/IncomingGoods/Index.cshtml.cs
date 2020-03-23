using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Inventory.IncomingGoods
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public IEnumerable<IncomingGood> IncomingGoodList { get; set; }
        public IncomingGood IncomingGoods { get; set; }
        public ProdInfo ProdInfo { get; set; }
        public Warehouse Warehouse { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SearchFromDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SearchToDate { get; set; }


        public async Task<IActionResult> OnGet(string SearchProdCode = null, DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            StringBuilder Param = new StringBuilder();


            
            if (SearchProdCode != null)
            {
                Param.Append(SearchProdCode);
            }
            Param.Append("&searchProdCode=");

            if (SearchFromDate != null)
            {
                Param.Append(SearchFromDate);
            }
            Param.Append("&SearchCreatedTime=");

            if (SearchToDate != null)
            {
                Param.Append(SearchToDate);
            }
            Param.Append("&SearchToDate=");

            if (SearchFromDate != null & SearchToDate != null & SearchProdCode == null)
            {
                IncomingGoodList = await _db.IncomingGood.Include(tr => tr.ProdInfo).Where(u => u.CreatedDateTime >= SearchFromDate & u.CreatedDateTime <= SearchToDate).ToListAsync();
             //   IncomingGoodList = _db.IncomingGood.Where(u => u.ProdInfo.ProdCode.ToLower().Contains(SearchProdCode.ToLower())).ToList();

            }
            else
            {
                if (SearchFromDate != null & SearchToDate != null & SearchProdCode != null)
                {
                    IncomingGoodList = await _db.IncomingGood.Include(tr => tr.ProdInfo).Where(u => u.ProdInfo.ProdCode.ToLower().Contains(SearchProdCode.ToLower()) & u.CreatedDateTime >= SearchFromDate & u.CreatedDateTime <= SearchToDate).ToListAsync();
                }
                else
                {
                    if (SearchFromDate == null & SearchToDate == null & SearchProdCode != null)
                    {
                        IncomingGoodList = await _db.IncomingGood.Include(tr => tr.ProdInfo).Where(u => u.ProdInfo.ProdCode.ToLower().Contains(SearchProdCode.ToLower())).ToListAsync();
                    }
                    else
                    {
 

                          IncomingGoodList = _db.IncomingGood.Include(tr => tr.Warehouse).Include(tr => tr.ProdInfo).Include(tr => tr.ApplicationUser)
                          .Where(tr => tr.CreatedDateTime > DateTime.Now.AddMonths(-1)).ToList().OrderBy(tr => tr.CreatedDateTime);
                    }
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

    }
}