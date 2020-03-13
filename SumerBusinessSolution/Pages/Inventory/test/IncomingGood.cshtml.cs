using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution
{
    public class tnewModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        private readonly IInventoryTrans _InveTrans;
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public tnewModel(ApplicationDbContext db, IInventoryTrans InveTrans)
        {
            _db = db;
            _InveTrans = InveTrans;

        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public IList<IncomingGood> ci { get; set; }

        [BindProperty]
        public string ProdCode { get; set; }

        [BindProperty]
        public IList<Warehouse> WarehouseList { get; set; }

        public ActionResult OnGet()
        {
            var ci1 = new IncomingGood { ProdId = 0, Qty = 0, WhId = 0, Note = "" } ;
            var ci2 = new IncomingGood { ProdId = 0, Qty = 0, WhId = 0, Note = "" };
            var ci3 = new IncomingGood { ProdId = 0, Qty = 0, WhId = 0, Note = "" };

            ci = new List<IncomingGood> { new IncomingGood { ProdId = 0, Qty = 0, WhId = 0, Note = "" } };

            ci.Add(ci1);
            ci.Add(ci2);
            ci.Add(ci3);

            WarehouseList = _db.Warehouse.ToList();
            return Page();
        }
        public ActionResult OnPost(List<IncomingGood> ci)
        {
           // int ProdId;

            // if (ModelState.IsValid)
            //  {
            //using (_db dc = new MyDatabaseEntities())
            //{
            //foreach (var i in ci)
            //{
            //    if(i == null)
            //    {
            //        break;
            //    }
            //    ProdId = _db.ProdInfo.FirstOrDefault(pro => pro.ProdCode == i.ProdInfo.ProdCode).Id;
            //    // _db.IncomingGood.Add(i);
               
            //}
             bool incomingoods = _InveTrans.CreateIncomingGoods(ci).GetAwaiter().GetResult();
            //_db.SaveChanges();
            StatusMessage = "Data successfully saved!";
            ModelState.Clear();
       
            // }
            // }
            return RedirectToPage("/Inventory/test/tnew");
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