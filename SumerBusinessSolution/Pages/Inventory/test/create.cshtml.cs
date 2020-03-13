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
    public class createModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        private readonly IInventoryTrans _InveTrans;
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public createModel(ApplicationDbContext db, IInventoryTrans InveTrans)
        {
            _db = db;
            _InveTrans = InveTrans;

        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public List<IncomingGood> ci { get; set; }

        [BindProperty]
        public string ProdCode { get; set; }

        public ActionResult OnGet()
        {
            ci = new List<IncomingGood> { new IncomingGood { ProdId = 0, Qty = 0, WhId = 0, Note = "" } };
            return Page();
        }
        public ActionResult OnPost(List<IncomingGood> ci)
        {
            int ProdId;
          
            // if (ModelState.IsValid)
            //  {
            //using (_db dc = new MyDatabaseEntities())
            //{
            foreach (var i in ci)
                {
                ProdId = _db.ProdInfo.FirstOrDefault(pro => pro.ProdCode == i.ProdInfo.ProdCode).Id;
                // _db.IncomingGood.Add(i);
                //bool incomingoods = _InveTrans.CreateIncomingGoods(i.WhId?? 0, ProdId, i.Qty, i.Note).GetAwaiter().GetResult();
                }
                //_db.SaveChanges();
                StatusMessage = "Data successfully saved!";
                ModelState.Clear();
                ci = new List<IncomingGood> { new IncomingGood { ProdId = 0, Qty = 0, WhId = 0, Note = "" } };
                // }
           // }
            return RedirectToPage("/Inventory/test/create");
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