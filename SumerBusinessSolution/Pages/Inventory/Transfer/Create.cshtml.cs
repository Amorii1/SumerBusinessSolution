using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Pages.Inventory.Transfer
{
    [Authorize(Roles = SD.AdminEndUser)]
    [Authorize]
    public class Create : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IInventoryTrans _InveTrans;

        public Create(ApplicationDbContext db, IInventoryTrans InveTrans)
        {
            _db = db;
            _InveTrans = InveTrans;
        }

        //public InvTransfer InvTransfer { get; set; }
        //public Warehouse Warehouse { get; set; }
        public IEnumerable<Warehouse> Warehouselist { get; set; }
        public IEnumerable<Warehouse> WarehouselistTo { get; set; }

        [Required]
        [BindProperty]
     [Display(Name ="رمز المنتج")]
        public string ProdCode { get; set; }
        [BindProperty]
      [Display(Name = "من المخزن ")]

        public int FromWhId { get; set; }
        [BindProperty]
        [Display(Name = "الى المخزن ")]

        public int ToWhId { get; set; }

        [Required]
        [BindProperty]

       [Display(Name = "الكمية ")]
        public double Qty { get; set; }
        [BindProperty]
       [Display(Name = "الملاحظات")]
        public string Note { get; set; }
        
        [TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public List<InvTransfer> InvT { get; set; }

        public void OnGet()
        {
            var InvT1 = new InvTransfer { ProdId = 0, Qty = 0, Note = "" };
            var InvT2 = new InvTransfer { ProdId = 0, Qty = 0, Note = "" };
            var InvT3 = new InvTransfer { ProdId = 0, Qty = 0, Note = "" };

            InvT = new List<InvTransfer> { new InvTransfer { ProdId = 0, Qty = 0, Note = "" } };

            InvT.Add(InvT1);  
            InvT.Add(InvT2);
            InvT.Add(InvT3);


            Warehouselist = _db.Warehouse.ToList();
            WarehouselistTo = _db.Warehouse.ToList().OrderByDescending(wh => wh.Id);

        }

 
        public IActionResult OnPost()
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
                 StatusMessage = _InveTrans.CreateInvTransfer(FromWhId, ToWhId, InvT).GetAwaiter().GetResult();
                //_db.SaveChanges();
             //   StatusMessage = StatusMessage;
              //  ModelState.Clear();

                // }
                // }
          
        

            //int ProdId;
            //try
            //{
            //     ProdId = _db.ProdInfo.FirstOrDefault(pro => pro.ProdCode == ProdCode).Id;
            //}
            //catch
            //{
            //    //StatusMessage = "Error! Product code can not be found";
            //    StatusMessage = "عذراً! رمز المنتج غير موجود";
            //    return RedirectToPage("/inventory/transfer/create");
            //}

            // StatusMessage = _InveTrans.CreateInvTransfer(ProdId, FromWhId, ToWhId, Qty, Note).GetAwaiter().GetResult();

            //if (invTransfer == true)
            //{
            //    StatusMessage = "New goods have been transfered successfully.";
            //}
            //else
            //{
            //    StatusMessage = "Error! New goods have not been transfered.";
            //}
            return RedirectToPage("/inventory/transfer/create");

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

      
    }
}