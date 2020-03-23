using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Inventory.Transfer
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public IEnumerable<InvTransfer> InvTransferList { get; set; }

         public InvTransfer InvTransfers { get; set; }

        public InvTransferHeader InvTransferHeader { get; set; }

        public ProdInfo ProdInfo { get; set; }
        public Warehouse Warehouse { get; set; }


        public  IActionResult OnGet(string searchCreateDateTime = null, string searchProdCode = null)
        {
            InvTransferList = _db.InvTransfer.Include(tr => tr.ProdInfo).Include(tr => tr.InvTransferHeader)
                .Include(tr=> tr.InvTransferHeader.FromWarehouse).Include(tr=> tr.InvTransferHeader.ToWarehouse)
                .Include(tr=> tr.InvTransferHeader.ApplicationUser)
               .Where(tr => tr.InvTransferHeader.CreatedDateTime > DateTime.Now.AddMonths(-1) & tr.InvTransferHeader.TransferStatus == SD.Approved).ToList().OrderBy(tr => tr.InvTransferHeader.CreatedDateTime);

            StringBuilder Param = new StringBuilder();

            Param.Append("&searchProdCode=");

            if (searchProdCode != null)
            {
                Param.Append(searchProdCode);
            }
            Param.Append("&searchCreateDateTime=");

            if (searchCreateDateTime != null)
            {
                Param.Append(searchCreateDateTime);
            }


            if (searchProdCode != null)
            {
                InvTransferList = _db.InvTransfer.Where(u => u.ProdInfo.ProdCode.ToLower().Contains(searchProdCode.ToLower())).ToList();

            }
            else
            {
                if (searchCreateDateTime != null)
                {

                 //   InvTransferList = _db.InvTransfer.Where(u => u.CreatedDateTime.ToString().Contains(searchCreateDateTime)).ToList();

                }

            }
            return Page();
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