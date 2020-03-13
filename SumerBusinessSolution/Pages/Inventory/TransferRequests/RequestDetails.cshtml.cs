using System;
using System.Collections.Generic;
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

namespace SumerBusinessSolution
{
    [Authorize]
    public class RequestDetailsModel : PageModel
    {
    
     
            private readonly ApplicationDbContext _db;
            private readonly IInventoryTrans _InveTrans;

            public RequestDetailsModel(ApplicationDbContext db, IInventoryTrans InveTrans)
            {
                _db = db;
                _InveTrans = InveTrans;
            }

            public ProdInfo ProdInfo { get; set; }

            [BindProperty]
            public IEnumerable<InvTransfer> InvTransferList { get; set; }

        public InvTransfer InvTransfer  { get; set; }
        public InvTransferHeader InvTransferHeader { get; set; }


            [BindProperty]
            public int ReqId { get; set; }
            [TempData]
            public string StatusMessage { get; set; }
            public ActionResult OnGet(int HeaderId)
            {
                InvTransferHeader = _db.InvTransferHeader.FirstOrDefault(h => h.Id == HeaderId);

                InvTransferList = _db.InvTransfer.Include(tr => tr.ProdInfo).Include(tr => tr.InvTransferHeader)
                .Include(tr => tr.InvTransferHeader.ApplicationUser)
                .Where(tr => tr.HeaderId == HeaderId).ToList().OrderBy(tr => tr.InvTransferHeader.CreatedDateTime);
 
                return Page();

            }
            public IActionResult OnPostApproveTransferRequests(int ReqId)
            {
                StatusMessage = _InveTrans.ApproveInvTransferRequest(ReqId).GetAwaiter().GetResult();
                return RedirectToPage("/Inventory/transferrequests/requestdetails");
            }
            public IActionResult OnPostRejectTransferRequests(int ReqId)
            {
                StatusMessage = _InveTrans.RejectInvTransferRequest(ReqId).GetAwaiter().GetResult();
                return RedirectToPage("/Inventory/transferrequests/requestdetails", new { ReqId = ReqId });
            }
            public IActionResult OnPostDeleteTransferRequestsLine(int ReqId)
            {
               StatusMessage = _InveTrans.DeleteInvTransferRequestLine(ReqId).GetAwaiter().GetResult();
               return RedirectToPage("/Inventory/transferrequests/requestdetails", new { ReqId = ReqId });
            }
        }
    }
