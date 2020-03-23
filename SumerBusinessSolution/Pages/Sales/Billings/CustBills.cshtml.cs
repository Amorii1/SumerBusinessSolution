using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Pages.Sales.Billings
{
    public class CustBillsModel : PageModel
    {


        private readonly ApplicationDbContext _db;
        private readonly ISalesTrans _SalesTrans;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public CustBillsModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }
        public BillHeader BillHeader { get; set; }

        [BindProperty]
        public List<BillHeader> BillHeaderList { get; set; }
        //public List<Customer> CustomerList { get; set; }


        [BindProperty]
        public int HeaderId { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SearchFromDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SearchToDate { get; set; }

        [TempData]
        public int NewCustId { get; set; }

        public async Task<IActionResult> OnGet(int CustId,DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            //  CustomerList = _db.Customer.ToList();
             BillHeaderList = await _db.BillHeader.Include(header => header.Customer).Where(header => header.CustId == CustId).ToListAsync();
           // NewCustId = CustId;
            //StringBuilder Param = new StringBuilder();


            //if (SearchFromDate != null)
            //{
            //    Param.Append(SearchFromDate);
            //}
            //Param.Append("&SearchCreatedTime=");

            //if (SearchToDate != null)
            //{
            //    Param.Append(SearchToDate);
            //}
            //Param.Append("&SearchToDate=");

            //Param.Append(NewCustId);
            //Param.Append("&CustId=");


            //if (SearchFromDate != null & SearchToDate != null)
            //{
            //    BillHeaderList = await _db.BillHeader.Include(header => header.Customer).Where(u => u.CustId == CustId & u.CreatedDataTime >= SearchFromDate & u.CreatedDataTime <= SearchToDate).ToListAsync();
            //}
            //else
            //{
            //    BillHeaderList = await _db.BillHeader.Include(header => header.Customer).Where(header => header.CustId == CustId).ToListAsync();
            //}

            //return RedirectToPage("/Sales/Billings/CustBills");
            return Page();
        }
        public IActionResult OnPostCloseBillManually(int HeaderId)
        {

            StatusMessage = _SalesTrans.CloseBillManually(HeaderId).GetAwaiter().GetResult();

            return RedirectToPage("/Sales/Reports/Bills");
        }
    }

}
