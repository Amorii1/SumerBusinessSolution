using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Inventory.TransferRequests
{
  //[Authorize(Roles =SD.AdminEndUser)]
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

        [BindProperty]
        public RoleAuth RoleAuth { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "من")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SearchFromDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "الى")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SearchToDate { get; set; }


        public async Task<IActionResult> OnGet(DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            StringBuilder Param = new StringBuilder();

            if (SearchFromDate != null)
            {
                Param.Append(SearchFromDate);
            }
            Param.Append("&SearchCreatedTime=");

            if (SearchToDate != null)
            {
                Param.Append(SearchToDate);
            }
            Param.Append("&SearchToDate=");


            if (SearchFromDate != null & SearchToDate != null)
            {
                // BillHeaderList = await _db.BillHeader.Include(header => header.Customer).Where(u => u.CreatedDataTime >= SearchFromDate & u.CreatedDataTime <= SearchToDate).ToListAsync();
                InvTransferHeaderList = _db.InvTransferHeader
                  .Include(tr => tr.FromWarehouse)
                  .Include(tr => tr.ToWarehouse)
                  .Include(tr => tr.ApplicationUser)
                  .Where(tr => tr.TransferStatus == SD.Pending & tr.CreatedDateTime >= SearchFromDate & tr.CreatedDateTime <= SearchToDate).ToList().OrderByDescending(tr => tr.CreatedDateTime);
            }
            else
            {
                InvTransferHeaderList = _db.InvTransferHeader
                .Include(tr => tr.FromWarehouse)
                .Include(tr => tr.ToWarehouse)
                .Include(tr => tr.ApplicationUser)
                .Where(tr => tr.TransferStatus == SD.Pending).ToList().OrderByDescending(tr => tr.CreatedDateTime);
            }


            RoleAuth = _db.RoleAuth.FirstOrDefault(ro => ro.RoleName == SD.SupervisorEndUser);

            return Page();

        }

        // SignalR code
        public async Task<IActionResult> OnGetPendingRequests()
        {
            return new JsonResult(await _InveTrans.GetPendingTransferRequests());
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
       
    }
}