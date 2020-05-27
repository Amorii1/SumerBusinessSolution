using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using System.ComponentModel.DataAnnotations;

namespace SumerBusinessSolution.Pages.Inventory.Reports
{
    [Authorize]
    [Authorize(Roles = SD.AdminUser)]
    public class InvTransactionsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public IEnumerable<InvTransaction> InvTransList { get; set; }

        [BindProperty]
        public InvTransaction InvTrans { get; set; }

        [BindProperty]
        public IList<Warehouse> WarehouseList { get; set; }

        [BindProperty]
        public int WhId{ get; set; }

        public InvTransactionsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [DataType(DataType.Date)]
        [Display(Name = "من")]
        public DateTime? SearchFromDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "الى")]
        public DateTime? SearchToDate { get; set; }

  

        public async Task<IActionResult> OnGet(string SearchProdCode = null, DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            WarehouseList = _db.Warehouse.ToList();

            StringBuilder Param = new StringBuilder();

            //Param.Append("&searchProdCode=");
            //Param.Append("&searchWh=");
            //Param.Append("&searchCreatedDate=");

            //if (WhId != "null")
            //{
            //    Param.Append(WhId);
            //}

            //if (SearchProdCode != null)
            //{
            //    Param.Append(SearchProdCode);
            //}

            //if (SearchCreatedDate != null)
            //{
            //    Param.Append(SearchCreatedDate);
            //}

            if (SearchFromDate != null & SearchToDate != null & SearchProdCode == null)
            {
                InvTransList = _db.InvTransaction
                    .Include(stk => stk.Warehouse)
                    .Include(stk => stk.ProdInfo)
                    .Where(tr => tr.CreatedDateTime >= SearchFromDate & tr.CreatedDateTime <= SearchToDate).ToList().OrderByDescending(tr => tr.CreatedDateTime);
            }
            else
            {
                if (SearchFromDate != null & SearchToDate != null & SearchProdCode != null)
                {
                    InvTransList = _db.InvTransaction
                        .Include(stk => stk.Warehouse)
                        .Include(stk => stk.ProdInfo)
                        .Where(tr => tr.ProdInfo.ProdCode.ToLower().Contains(SearchProdCode.ToLower()) & tr.CreatedDateTime >= SearchFromDate & tr.CreatedDateTime <= SearchToDate).ToList().OrderByDescending(tr => tr.CreatedDateTime);
                }
                else
                {
                    if (SearchFromDate == null & SearchToDate == null & SearchProdCode != null)
                    {
                        InvTransList = _db.InvTransaction
                       .Include(stk => stk.Warehouse)
                       .Include(stk => stk.ProdInfo)
                       .Where(tr => tr.ProdInfo.ProdCode.ToLower().Contains(SearchProdCode.ToLower())).ToList().OrderByDescending(tr => tr.CreatedDateTime);
                    }
                    else
                    {
                        InvTransList = _db.InvTransaction
                       .Include(stk => stk.Warehouse)
                       .Include(stk => stk.ProdInfo)
                       .Where(tr => tr.CreatedDateTime > DateTime.Now.AddMonths(-1)).ToList().OrderByDescending(tr => tr.CreatedDateTime);

                    }
                }
            }


            //        if (SearchProdCode == null & WhId ==  null  & SearchCreatedDate == null)
            //{
            //    InvTransList = await _db.InvTransaction.Include(tr => tr.Warehouse).Include(tr => tr.ProdInfo)
            //   .Where(tr => tr.CreatedDateTime > DateTime.Now.AddMonths(-3)).ToListAsync();

            //    return Page();
            //}


            //if (SearchProdCode != null & WhId != "null" & SearchCreatedDate != null)
            //{
            //   // InvTransList = await _db.InvTransaction.Include(stk => stk.Warehouse).Include(stk => stk.ProdInfo)
            //   //.Where(stk => stk.ProdInfo.ProdCode == SearchProdCode 
            //   //& stk.Warehouse.Id.ToString() == WhId & stk.CreatedDateTime.ToString().Contains(SearchCreatedDate)).ToListAsync();
            //}
            //else
            //{
            //    if (SearchProdCode != null & WhId != "null" & SearchCreatedDate == null)
            //    {
            //        InvTransList = await _db.InvTransaction.Include(stk => stk.Warehouse).Include(stk => stk.ProdInfo)
            //        .Where(stk => stk.ProdInfo.ProdCode == SearchProdCode
            //        & stk.Warehouse.Id.ToString() == WhId ).ToListAsync();
            //    }
            //    else if(SearchProdCode != null & WhId == "null" & SearchCreatedDate == null)
            //    {
            //        InvTransList = await _db.InvTransaction.Include(stk => stk.Warehouse).Include(stk => stk.ProdInfo)
            //        .Where(stk => stk.ProdInfo.ProdCode == SearchProdCode).ToListAsync();
            //    }
            //    else if (SearchProdCode == null & WhId != "null" & SearchCreatedDate != null)
            //    {
            //        InvTransList = await _db.InvTransaction.Include(stk => stk.Warehouse).Include(stk => stk.ProdInfo)
            //        .Where(stk => stk.Warehouse.Id.ToString() == WhId & stk.CreatedDateTime.ToString().Contains(SearchCreatedDate)).ToListAsync();
            //    }
            //    else if (SearchProdCode == null & WhId == "null" & SearchCreatedDate != null)
            //    {
            //        InvTransList = await _db.InvTransaction.Include(stk => stk.Warehouse).Include(stk => stk.ProdInfo)
            //      .Where(stk => stk.CreatedDateTime.ToString().Contains(SearchCreatedDate)).ToListAsync();
            //    }
            //    else if (SearchProdCode != null & WhId == "null" & SearchCreatedDate != null)
            //    {
            //        InvTransList = await _db.InvTransaction.Include(stk => stk.Warehouse).Include(stk => stk.ProdInfo)
            //        .Where(stk => stk.ProdInfo.ProdCode == SearchProdCode
            //        &  stk.CreatedDateTime.ToString().Contains(SearchCreatedDate)).ToListAsync();
            //    }
            //    else if (SearchProdCode == null & WhId != "null" & SearchCreatedDate == null)
            //    {
            //        InvTransList = await _db.InvTransaction.Include(stk => stk.Warehouse).Include(stk => stk.ProdInfo)
            //        .Where(stk=> stk.Warehouse.Id.ToString() == WhId).ToListAsync();
            //    }
            //}
  

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
 