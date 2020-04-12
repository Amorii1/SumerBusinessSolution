using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Pages.Inventory.Transfer
{
    [Authorize(Roles =SD.AdminEndUser)]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IInventoryTrans _InveTrans;

        public CreateModel(ApplicationDbContext db, IInventoryTrans InveTrans)
        {
            _db = db;
            _InveTrans = InveTrans;
           
        }

        //public InvTransfer InvTransfer { get; set; }
        //public Warehouse Warehouse { get; set; }

        [BindProperty]
        public IEnumerable<Warehouse> WhFromlist { get; set; }

        [BindProperty]
        public IEnumerable<Warehouse> WhTolist { get; set; }

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
       // [BindProperty]
       //[Display(Name = "الملاحظات")]
       // public string Note { get; set; }
        
        [TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public List<InvTransfer> InvT { get; set; }

        public ActionResult OnGet()
        {
            InvT = new List<InvTransfer> { new InvTransfer { ProdId = 0, Qty = 0, Note = "" } };

            WhFromlist = _db.Warehouse.Where(wh=> wh.Active == true).OrderByDescending(wh => wh.WhType.Type).ToList();
            WhTolist = _db.Warehouse.Where(wh => wh.Active == true).OrderBy(wh => wh.WhType.Type).ToList();

            return Page();
        }

 
        public IActionResult OnPost(List<InvTransfer> InVT)
        {
  
            StatusMessage = _InveTrans.CreateInvTransfer(FromWhId, ToWhId, InvT).GetAwaiter().GetResult();
      
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