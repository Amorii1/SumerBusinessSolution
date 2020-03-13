using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Inventory.TransferRequests
{
 //   [Authorize(Roles =SD.AdminEndUser)]
   // [Authorize(Roles = SD.SupervisorEndUser)]
   [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IInventoryTrans _InveTrans;

        public IndexModel(ApplicationDbContext db, IInventoryTrans InveTrans)
        {
            _db = db;
            _InveTrans = InveTrans;
        }

        public ProdInfo ProdInfo { get; set; }

        [BindProperty]
        public IEnumerable<InvTransferHeader> InvTransferHeaderList { get; set; }

         public InvTransferHeader InvTransferHeader { get; set; }

 
        [BindProperty]
        public int ReqId { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGet()
        {
 
            InvTransferHeaderList = _db.InvTransferHeader
                .Include(tr => tr.FromWarehouse).Include(tr => tr.ToWarehouse)
               .Include(tr => tr.ApplicationUser)
              .Where(tr => tr.TransferStatus == SD.Pending).ToList().OrderBy(tr => tr.CreatedDateTime);



            return Page();

        }
        public  IActionResult  OnPostApproveTransferRequests(int ReqId)
        {

            StatusMessage = _InveTrans.ApproveInvTransferRequest(ReqId).GetAwaiter().GetResult();

            return RedirectToPage("/Inventory/transferrequests/index");
        }
 
        public IActionResult OnPostRejectTransferRequests(int ReqId)
        {

            StatusMessage = _InveTrans.RejectInvTransferRequest(ReqId).GetAwaiter().GetResult();
 
            return RedirectToPage("/Inventory/transferrequests/index");
        }
        public IActionResult OnPostDeleteTransferRequests(int ReqId) 
        {

            StatusMessage = _InveTrans.DeleteInvTransferRequestHeader(ReqId).GetAwaiter().GetResult();       

            return RedirectToPage("/Inventory/transferrequests/index");
        }
    }
}