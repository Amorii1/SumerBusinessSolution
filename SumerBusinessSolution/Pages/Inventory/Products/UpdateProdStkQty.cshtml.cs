using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Pages.Inventory.Products
{
    [Authorize]
    public class UpdateProdStkQtyModel : PageModel
    {
   

        private readonly ApplicationDbContext _db;
        private readonly IInventoryTrans _InveTrans;
        public UpdateProdStkQtyModel(ApplicationDbContext db, IInventoryTrans InveTrans)
        {
            _db = db;
            _InveTrans = InveTrans;
        }
        [BindProperty]
        public InvStockQty InvStkQty { get; set; }
        [Display(Name ="اسم المخزن")]
        public string WhName { get; set; }

        [BindProperty]
        [Display(Name = "الكميه الجديده")]

        public double NewQty { get; set; }
        [Display(Name = "رمز المنتج")]

        public string ProdCode { get; set; }


        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet(int? StkId)
        {
            InvStkQty = _db.InvStockQty.Include(stk => stk.ProdInfo).Include(stk => stk.Warehouse)
                .FirstOrDefault(stk => stk.Id == StkId);
 
        }

        public async Task<IActionResult> OnPost(int StkId)
        {

            bool UpdateStk = _InveTrans.UpdateProdStkQty(StkId, NewQty).GetAwaiter().GetResult();
             string searchProdCodew = InvStkQty.ProdInfo.ProdCode;

            if (UpdateStk == true)
            {
                StatusMessage = "Product Qty updated";
            }
            else
            {
                StatusMessage = "Error! Qty not updated";
            }

            return RedirectToPage("/inventory/Products/UpdateProdStkQty", new { StkId = StkId });
        }
    }
}

  