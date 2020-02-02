using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Pages.Products
{
    public class InGoodModel : PageModel
    {
         private readonly ApplicationDbContext _db;
        private IInventoryTrans _invTrans;


       // public InventoryTrans InvObj { get; set; }
        public InGoodModel(IInventoryTrans invTrans, ApplicationDbContext  db)
        {
            _db = db;
            _invTrans = invTrans;
        }
        [ModelBinder]
        public IncomingGood InGood { get; set; }
        [BindProperty]
        public string ProdCode { get; set; }
        public void OnGet()
        {
         
        }
        public JsonResult OnGetSearchNow(string term)
        {
            // ServiceTypeList = _db.ServiceType.Where(st => st.)

            if (term == null)
            {
                return new JsonResult("Not Found");
            }
            IQueryable<string> lstProd = from s in _db.ProdInfo
                                            where (s.ProdCode.Contains(term))
                                            select s.ProdCode;
            return new JsonResult(lstProd);
        }

        public async Task<IActionResult> OnPost()
        {
            int ProdId = _db.ProdInfo.FirstOrDefault(pro => pro.ProdCode == ProdCode).Id;

            bool x = _invTrans.CreateIncomingGoods(3, ProdId, 240, ProdCode).GetAwaiter().GetResult();

            // bool y = _invTrans.CreatePendingInvTransfer(4, 3, 6, 60).GetAwaiter().GetResult();
            //  bool x = _invTrans.ApproveInvTransferRequest(3).GetAwaiter().GetResult();
            //_invTrans.CreateProdInWh(5);
            //_invTrans.CheckProdCodeExist(ProdCode).GetAwaiter().GetResult();
            return Page();
        }

    }
}