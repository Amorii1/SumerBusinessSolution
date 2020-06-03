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

namespace SumerBusinessSolution
{
    [Authorize]
    public class ProductInquiryModel : PageModel
    {
            private readonly ApplicationDbContext _db;

            [BindProperty]
            public IList<InvStockQty> InvStockQtyList { get; set; }
            public ProductInquiryModel(ApplicationDbContext db)
            {
                _db = db;
            }
            public async Task<IActionResult> OnGet(string searchProdCode = null)
            {
                StringBuilder Param = new StringBuilder();
                Param.Append("&searchProdCode=");

                if (searchProdCode != null)
                {
                    Param.Append(searchProdCode);
                }


                InvStockQtyList = await _db.InvStockQty
                .Include(stk => stk.Warehouse)
                .Include(stk => stk.ProdInfo)
                .Where(stk => stk.ProdInfo.ProdCode == searchProdCode).ToListAsync();

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
