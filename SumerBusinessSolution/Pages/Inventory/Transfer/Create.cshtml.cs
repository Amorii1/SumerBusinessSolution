using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Pages.Inventory.Transfer
{
    //   [Authorize]
    public class Create : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IInventoryTrans _InveTrans;

        public Create(ApplicationDbContext db, IInventoryTrans InveTrans)
        {
            _db = db;
            _InveTrans = InveTrans;
        }
        [BindProperty]
        public IncomingGood IncomingGood { get; set; }
        public IList<IncomingGood> IncomingGoodlist { get; set; }

        public InvTransfer InvTransfer { get; set; }
        public Warehouse Warehouse { get; set; }
        public IList<Warehouse> Warehouselist { get; set; }
        public ProdInfo ProdInfo { get; set; }
        [BindProperty]
        public string ProdCode { get; set; }
        [BindProperty]
        public int FromWhId { get; set; }
        [BindProperty]
        public int ToWhId { get; set; }

        [BindProperty]
        public int ProdId { get; set; }
        [BindProperty]
        public double Qty { get; set; }
        [BindProperty]
        public string Note { get; set; }
        


        [TempData]
        public string StatusMessage { get; set; }


        public IActionResult OnGet()
        {
          
            Warehouselist = _db.Warehouse.ToList();
       
            return Page();

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
            int ProdId = _db.ProdInfo.FirstOrDefault(pro => pro.ProdCode == ProdCode).Id;
            bool invTransfer = _InveTrans.CreateInvTransfer(ProdId, FromWhId, ToWhId, Qty, Note).GetAwaiter().GetResult();

            if (invTransfer == true)
            {
                StatusMessage = "New goods have been transfered successfully.";
            }
            else
            {
                StatusMessage = "New goods havenot been transfered successfully.";
            }


            return Page();

        }
    }
}