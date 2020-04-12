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

namespace SumerBusinessSolution.Inventory.Transfer
{
    [Authorize]
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

        [BindProperty]
        public RoleAuth RoleAuth { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "من")]
        public DateTime SearchFromDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "الى")]
        public DateTime SearchToDate { get; set; }

        public async Task<IActionResult> OnGet(string SearchProdCode = null, DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
         
            StringBuilder Param = new StringBuilder();

            Param.Append("&searchProdCode=");

            if (SearchProdCode != null)
            {
                Param.Append(SearchProdCode);
            }
            Param.Append("&searchCreateDateTime=");

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

            if (SearchFromDate != null & SearchToDate != null & SearchProdCode == null)
            {
                InvTransferList =   _db.InvTransfer
                      .Include(tr => tr.ProdInfo)
                      .Include(tr => tr.InvTransferHeader)
                      .Include(tr => tr.InvTransferHeader.FromWarehouse)
                      .Include(tr => tr.InvTransferHeader.ToWarehouse)
                      .Include(tr => tr.InvTransferHeader.ApplicationUser)
                      .Where(tr => tr.InvTransferHeader.CreatedDateTime >= SearchFromDate & tr.InvTransferHeader.CreatedDateTime <= SearchToDate & tr.InvTransferHeader.TransferStatus == SD.Approved).ToList().OrderByDescending(tr => tr.InvTransferHeader.CreatedDateTime);

            }
            else
            {
                if (SearchFromDate != null & SearchToDate != null & SearchProdCode != null)
                {
                    InvTransferList =   _db.InvTransfer
                   .Include(tr => tr.ProdInfo)
                   .Include(tr => tr.InvTransferHeader)
                   .Include(tr => tr.InvTransferHeader.FromWarehouse)
                   .Include(tr => tr.InvTransferHeader.ToWarehouse)
                   .Include(tr => tr.InvTransferHeader.ApplicationUser)
                   .Where(tr => tr.ProdInfo.ProdCode.ToLower().Contains(SearchProdCode.ToLower()) & tr.InvTransferHeader.CreatedDateTime >= SearchFromDate & tr.InvTransferHeader.CreatedDateTime <= SearchToDate & tr.InvTransferHeader.TransferStatus == SD.Approved).ToList().OrderByDescending(tr => tr.InvTransferHeader.CreatedDateTime);
                }
                else
                {
                    if (SearchFromDate == null & SearchToDate == null & SearchProdCode != null)
                    {
                        InvTransferList =   _db.InvTransfer
                           .Include(tr => tr.ProdInfo)
                           .Include(tr => tr.InvTransferHeader)
                           .Include(tr => tr.InvTransferHeader.FromWarehouse)
                           .Include(tr => tr.InvTransferHeader.ToWarehouse)
                           .Include(tr => tr.InvTransferHeader.ApplicationUser)
                           .Where(tr => tr.ProdInfo.ProdCode.ToLower().Contains(SearchProdCode.ToLower()) & tr.InvTransferHeader.TransferStatus == SD.Approved).ToList().OrderByDescending(tr => tr.InvTransferHeader.CreatedDateTime);
                    }
                    else
                    {
                        InvTransferList = _db.InvTransfer
                        .Include(tr => tr.ProdInfo)
                        .Include(tr => tr.InvTransferHeader)
                        .Include(tr => tr.InvTransferHeader.FromWarehouse)
                        .Include(tr => tr.InvTransferHeader.ToWarehouse)
                        .Include(tr => tr.InvTransferHeader.ApplicationUser)
                        .Where(tr => tr.InvTransferHeader.CreatedDateTime > DateTime.Now.AddMonths(-1) & tr.InvTransferHeader.TransferStatus == SD.Approved).ToList().OrderByDescending(tr => tr.InvTransferHeader.CreatedDateTime);
                    }
                }
            }
            RoleAuth = _db.RoleAuth.FirstOrDefault(ro => ro.RoleName == SD.SupervisorEndUser);

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