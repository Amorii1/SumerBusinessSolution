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

namespace SumerBusinessSolution.Pages.Inventory.Reports
{
    [Authorize]

    public class WhProdQtyModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public IList<InvStockQty> InvStockQtyList { get; set; }

        [BindProperty]
        public IList<Warehouse> WarehouseList { get; set; }

        [BindProperty]
        public int WhId { get; set; }

        public WhProdQtyModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGet(string WhId = null, string SearchProdCode = null)
        {
            WarehouseList = _db.Warehouse.ToList();

            StringBuilder Param = new StringBuilder();
            Param.Append("&searchWh=");
            Param.Append("&searchProdCode=");

            if (WhId != null)
            {
                Param.Append(WhId);
            }

            if (SearchProdCode != null)
            {
                Param.Append(SearchProdCode);
            }

            if(SearchProdCode != null)
            {
                InvStockQtyList = await _db.InvStockQty.Include(stk => stk.Warehouse).Include(stk => stk.ProdInfo)
               .Where(stk => stk.Warehouse.Id.ToString() == WhId & stk.ProdInfo.ProdCode == SearchProdCode & stk.Qty > 0).ToListAsync();
            }
            else 
            {
                InvStockQtyList = await _db.InvStockQty.Include(stk => stk.Warehouse).Include(stk => stk.ProdInfo)
                .Where(stk => stk.Warehouse.Id.ToString() == WhId  & stk.Qty > 0).ToListAsync();

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
 