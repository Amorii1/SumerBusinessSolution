﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Pages.Inventory.Products
{
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
        public string WhName { get; set; }

        [BindProperty]
        public double NewQty { get; set; }

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

  