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
using Microsoft.AspNetCore.SignalR;
using SumerBusinessSolution.Hubs;
namespace SumerBusinessSolution.Pages.Inventory.TransferRequests
{
   [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IInventoryTrans _InveTrans;
        private readonly IHubContext<NotificationHub> _hubContext;
        public CreateModel(ApplicationDbContext db, IInventoryTrans InveTrans, IHubContext<NotificationHub> hubContext)
        {
            _db = db;
            _InveTrans = InveTrans;
            _hubContext = hubContext;
        }

        public IEnumerable<Warehouse> WhFromlist { get; set; }
        public IEnumerable<Warehouse> WhTolist { get; set; }

        [Required]
        [BindProperty]
         [Display(Name = "رمز المنتج")]
        public string ProdCode { get; set; }
        [BindProperty]
       [Display(Name = "من المخزن ")]

        public int FromWhId { get; set; }
        [BindProperty]
        [Display(Name = "الى المخزن ")]

        public int ToWhId { get; set; }

        [Required]
        [BindProperty]
       [Display(Name = "الكمية")]
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
            InvT = new List<InvTransfer> { new InvTransfer { ProdId = 0, Qty = 0, Note = "" } };

        //    WhFromlist = _db.Warehouse.ToList();
        //    WhTolist = _db.Warehouse.ToList().OrderByDescending(wh => wh.Id);

            WhFromlist = _db.Warehouse.Where(wh => wh.Active == true).OrderByDescending(wh => wh.WhType.Type).ToList();
            WhTolist = _db.Warehouse.Where(wh => wh.Active == true).OrderBy(wh => wh.WhType.Type).ToList();
        }

        public IActionResult OnPost()
        {
            StatusMessage = _InveTrans.CreateInvTransferRequest(FromWhId, ToWhId, Note, InvT, _hubContext).GetAwaiter().GetResult();

            return RedirectToPage("/inventory/transferrequests/create");
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