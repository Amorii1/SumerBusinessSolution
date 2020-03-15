using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution
{
    [Authorize]
    public class IndexModel : PageModel
    {

         private readonly ApplicationDbContext _db;
         private readonly ISalesTrans _SalesTrans;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public IndexModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }
            public BillHeader BillHeader { get; set; }

            [BindProperty]
            public List<BillHeader> BillHeaderList { get; set; }
            public List<Customer> CustomerList { get; set; }


            [BindProperty]
            public int HeaderId { get; set; }

            [TempData]
            public string StatusMessage { get; set; }
            public void OnGet()
            {
                CustomerList = _db.Customer.ToList();
                BillHeaderList = _db.BillHeader.Where(header => header.Status == SD.OpenBill).ToList();
            }
            //public IActionResult OnPostApproveTransferRequests(int ReqId)
            //{

            //    StatusMessage = _InveTrans.ApproveInvTransferRequest(ReqId).GetAwaiter().GetResult();

            //    return RedirectToPage("/Inventory/transferrequests/index");
            //}

            //public IActionResult OnPostRejectTransferRequests(int ReqId)
            //{

            //    StatusMessage = _InveTrans.RejectInvTransferRequest(ReqId).GetAwaiter().GetResult();

            //    return RedirectToPage("/Inventory/transferrequests/index");
            //}
            //public IActionResult OnPostDeleteTransferRequests(int ReqId)
            //{

            //    StatusMessage = _InveTrans.DeleteInvTransferRequestHeader(ReqId).GetAwaiter().GetResult();

            //    return RedirectToPage("/Inventory/transferrequests/index");
            //}
    }
}
 