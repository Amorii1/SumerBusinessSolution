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
        public RoleAuth RoleAuth { get; set; }


        [BindProperty]
            public int ReqId { get; set; }
            [TempData]
            public string StatusMessage { get; set; }
            public ActionResult OnGet(int ReqId)
            {
                InvTransferHeader = _db.InvTransferHeader.Include(h=> h.FromWarehouse).Include(h=> h.ToWarehouse)
                .Include(h=> h.ApplicationUser)
                .FirstOrDefault(h => h.Id == ReqId);

                InvTransferList = _db.InvTransfer.Include(tr => tr.ProdInfo).Include(tr => tr.InvTransferHeader)
                .Include(tr => tr.InvTransferHeader.ApplicationUser)
                .Where(tr => tr.HeaderId == ReqId).ToList().OrderBy(tr => tr.InvTransferHeader.CreatedDateTime);

            RoleAuth = _db.RoleAuth.FirstOrDefault(ro => ro.RoleName == SD.SupervisorEndUser);

            return Page();

            }
            public IActionResult OnPostApproveTransferRequests(int ReqId)
            {
                StatusMessage = _InveTrans.ApproveInvTransferRequest(ReqId).GetAwaiter().GetResult();
                return RedirectToPage("/Inventory/transferrequests/index");
            }
            public IActionResult OnPostRejectTransferRequests(int ReqId)
            {
                StatusMessage = _InveTrans.RejectInvTransferRequest(ReqId).GetAwaiter().GetResult();
                return RedirectToPage("/Inventory/transferrequests/index");
            }
            public IActionResult OnPostDeleteTransferRequestsLine(int LineId, int ReqId)
            {
               StatusMessage = _InveTrans.DeleteInvTransferRequestLine(LineId).GetAwaiter().GetResult();
               return RedirectToPage("/Inventory/transferrequests/requestdetails", new { ReqId = ReqId });
            }

        public IActionResult OnPostDeleteTransferRequests(int ReqId)
        {

            StatusMessage = _InveTrans.DeleteInvTransferRequestHeader(ReqId).GetAwaiter().GetResult();

            return RedirectToPage("/Inventory/transferrequests/index");
        }
    }
    }
