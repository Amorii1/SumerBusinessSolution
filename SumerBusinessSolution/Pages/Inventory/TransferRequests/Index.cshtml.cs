using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Inventory.TransferRequests
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IInventoryTrans _InveTrans;

        public IndexModel(ApplicationDbContext db, IInventoryTrans InveTrans)
        {
            _db = db;
            _InveTrans = InveTrans;
        }
        [BindProperty]
        public IList<InvTransfer> InvTransferList { get; set; }
        [BindProperty]
        public int ReqId { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGet()
        {
            InvTransferList =await  _db.InvTransfer
               .Where(tr => tr.TransferStatus==SD.Pending ).ToListAsync();

            return Page();

        }
        public IActionResult OnPostApproveTransferRequests()
        {

            bool InvTransfer = _InveTrans.ApproveInvTransferRequest(ReqId).GetAwaiter().GetResult();
            if (InvTransfer == true)
            {
                StatusMessage = "New goods have been Transfered successfully.";

            }
            else
            {
                StatusMessage = "New goods have not been transfered successfully.";

            }





            return Page();
        }
        public IActionResult OnPostRejectTransferRequests()
        {

            bool InvTransfer = _InveTrans.RejectInvTransferRequest(ReqId).GetAwaiter().GetResult();
            if (InvTransfer == true)
            {
                StatusMessage = "New goods have not been Transfered successfully.";

            }
            else
            {
                StatusMessage = "New goods have been transfered successfully.";

            }

            return Page();
        }
        public IActionResult OnPostDeleteTransferRequests()
        {

            bool InvTransfer = _InveTrans.DeleteInvTransferRequest(ReqId).GetAwaiter().GetResult();
            if (InvTransfer == true)
            {
                StatusMessage = "New goods have  been deleted successfully.";

            }
            else
            {
                StatusMessage = "New goods have been deleted successfully.";

            }

            return Page();
        }
    }
}