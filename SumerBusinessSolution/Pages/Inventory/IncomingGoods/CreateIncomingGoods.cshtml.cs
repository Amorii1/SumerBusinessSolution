using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution
{
    //[Authorize]
    public class CreateIncomingGoodsModel : PageModel
    {
      
            private readonly ApplicationDbContext _db;
            private readonly IInventoryTrans _InveTrans;

            public CreateIncomingGoodsModel(ApplicationDbContext db, IInventoryTrans InveTrans)
            {
                _db = db;
                _InveTrans = InveTrans;
            }
            [BindProperty]
            public IncomingGood IncomingGood { get; set; }
            public IList<IncomingGood> IncomingGoodlist { get; set; }

            public IList<ProdInfo> ProdInfo { get; set; }
            public Warehouse Warehouse { get; set; }

        [BindProperty]
        public IList<Warehouse> WarehouseList { get; set; }
        [BindProperty]
            public int WhId { get; set; }
            [BindProperty]
            public int ProdId { get; set; }
            [BindProperty]
            public double Qty { get; set; }
            [BindProperty]
            public string Note { get; set; }
        [BindProperty]
        public string ProdCode { get; set; }
        [TempData]
            public string StatusMessage { get; set; }


        public void OnGet()
        {
            //  Warehouse = _db.Warehouse.FirstOrDefault();
           WarehouseList = _db.Warehouse.ToList();
          //  ProdInfo = _db.ProdInfo.Where(p => p.Id == ProdId).ToList();

         //   return Page();

        }

        public JsonResult OnGetSearchNow(string term)
        {
            //   ProdInfo = _db.ProdInfo.Where(w => w.Id == ProdId).ToList();
            if (term == null)
            {
                return new JsonResult("Not Found");
            }
            IQueryable<string> lstProdCode = from P in _db.ProdInfo
                                             where (P.ProdCode.Contains(term))
                                             select P.ProdCode;
            return new JsonResult(lstProdCode);

        }

        public IActionResult OnPost()
            {
            //WarehouseList = _db.Warehouse.ToList();

            int ProdId = _db.ProdInfo.FirstOrDefault(pro=>pro.ProdCode==ProdCode).Id;
                bool incomingoods = _InveTrans.CreateIncomingGoods(WhId, ProdId, Qty, Note).GetAwaiter().GetResult();

                if (incomingoods == true)
                {
                    StatusMessage = "New goods have been added successfully.";
                }
                else
                {
                    StatusMessage = "New goods havenot been added successfully.";
                }


                return RedirectToPage("/inventory/incominggoods/createincominggoods");

            }
        
    }
}
   