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
       // private readonly ApplicationDbContext _db;
        private IInventoryTrans _invTrans;

       // public InventoryTrans InvObj { get; set; }
        public InGoodModel(IInventoryTrans invTrans)
        {
           // _db = db;
            _invTrans = invTrans;
        }
        [ModelBinder]
        public IncomingGood InGood { get; set; }
        public void OnGet()
        {
         
        }

        public async Task<IActionResult> OnPost()
        {
               bool x =  _invTrans.CreateIncomingGoods(3, 5, 240, "Working").GetAwaiter().GetResult() ;
            // bool y = _invTrans.CreatePendingInvTransfer(4, 3, 6, 60).GetAwaiter().GetResult();
            //  bool x = _invTrans.ApproveInvTransferRequest(3).GetAwaiter().GetResult();
            //_invTrans.CreateProdInWh(5);
            _invTrans.CheckProdCodeExist("0002").GetAwaiter().GetResult();
            return Page();
        }

    }
}