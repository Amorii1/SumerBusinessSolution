using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;
using SumerBusinessSolution.Utility;

namespace SumerBusinessSolution
{
    [Authorize(Roles = SD.AdminUser)]
    public class ExternalSalesAnalysisModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ISalesTrans _SalesTrans;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public ExternalSalesAnalysisModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }
        public ExternalBillHeader ExternalBillHeader { get; set; }

        [BindProperty]
        public IEnumerable<ExternalBillHeader> ExternalBillHeaderList { get; set; }

        [BindProperty]
        public IEnumerable<ExternalBillItems> ExternalBillItemsList { get; set; }
        public List<Customer> CustomerList { get; set; }


        [BindProperty]
        public int HeaderId { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "من")]
        public DateTime? SearchFromDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "الى")]
        public DateTime? SearchToDate { get; set; }

        [BindProperty]
        [Display(Name = "الربح الصافي")]

        public double TotalNetAmt { get; set; }
        [BindProperty]
        [Display(Name = "التكلفة المدفوعة")]

        public double TotalPaidAmt { get; set; }

        [BindProperty]
        [Display(Name = "التكلفة غير المدفوعة")]

        public double TotalUnpaidAmt { get; set; }

        [BindProperty]
        [Display(Name = "الربح الصافي من الياسين")]

        public double TotalNetAmtExternal { get; set; } // total net amt of all external items
 
        [BindProperty]
        [Display(Name = "العائد الكلي")]

        public double TotalRevenue { get; set; }
        public IActionResult OnGet(string CustomerName = null, DateTime? SearchFromDate = null, DateTime? SearchToDate = null, int? BillNo = null)
        {

            CultureInfo culture = new CultureInfo("pt-BR");

            StringBuilder Param = new StringBuilder();

            Param.Append("&SearchCustomer=");

            if (CustomerName != null)
            {
                Param.Append(CustomerName);
            }
            Param.Append("&CustomerName=");

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

            if (BillNo != null)
            {
                Param.Append(BillNo);
            }
            Param.Append("&BillNo=");

            if (BillNo != null)
            {
                ExternalBillHeaderList = _db.ExternalBillHeader
                    .Include(header => header.Customer)
                    .Where(u => u.Id == BillNo).ToList();
            }
            else if (SearchFromDate != null & SearchToDate != null & CustomerName == null)
            {
                ExternalBillHeaderList = _db.ExternalBillHeader
                    .Include(header => header.Customer)
                    .Where(u => u.CreatedDataTime >= SearchFromDate & u.CreatedDataTime <= SearchToDate).ToList()
                     .OrderByDescending(header => header.CreatedDataTime);
            }
            else
            {
                if (SearchFromDate != null & SearchToDate != null & CustomerName != null)
                {
                    ExternalBillHeaderList = _db.ExternalBillHeader
                        .Include(header => header.Customer)
                        .Where(u => u.Customer.CompanyName.ToLower().Contains(CustomerName.ToLower()) & u.CreatedDataTime >= SearchFromDate & u.CreatedDataTime <= SearchToDate).ToList()
                        .OrderByDescending(header => header.CreatedDataTime);
                }
                else
                {
                    if (SearchFromDate == null & SearchToDate == null & CustomerName != null)
                    {
                        ExternalBillHeaderList = _db.ExternalBillHeader
                            .Include(header => header.Customer)
                            .Where(u => u.Customer.CompanyName.ToLower().Contains(CustomerName.ToLower())).ToList()
                            .OrderByDescending(header => header.CreatedDataTime);

                    }
                    else
                    {
                        ExternalBillHeaderList = _db.ExternalBillHeader
                               .Include(header => header.Customer)
                               .Where(header => header.CreatedDataTime >= DateTime.Now.AddMonths(-1)).ToList()
                               .OrderByDescending(header => header.CreatedDataTime);
                    }
                }

            }


            TotalNetAmt = 0;
            TotalPaidAmt = 0;
            TotalRevenue = 0;
            TotalNetAmtExternal = 0;
       
            foreach (ExternalBillHeader Bill in ExternalBillHeaderList)
            {
                TotalNetAmt += Bill.TotalNetAmt;
                TotalPaidAmt += Bill.PaidAmt;

                ExternalBillItemsList = _db.ExternalBillItems
                    .Include(it => it.ProdInfo)
                    .Where(it => it.HeaderId == Bill.Id).ToList();

                double TotalProdCost = 0;
                double TotalSoldPrice = 0;
                foreach (ExternalBillItems Item in ExternalBillItemsList)
                {
                    // in case this is not a external item, which means the item is already added to the store
                    // and its price has been set
                    if(Item.IsExternal == false)
                    {
                        TotalProdCost += (Item.ProdInfo.CostPrice) * Item.Qty;
                    }
                    else  // else if it is an external item then the user will enter the cost while creating the bill
                    {
                        if(Item.CostPrice != null)
                        {
                            TotalProdCost += (Item.CostPrice ?? 0) * Item.Qty;
                            TotalNetAmtExternal += Item.TotalAmt;
                        }
                       
                    }
                   
                    TotalSoldPrice += Item.TotalAmt;
                }

                TotalUnpaidAmt += Bill.TotalNetAmt - Bill.PaidAmt;
                TotalRevenue += (TotalSoldPrice - TotalProdCost) - Bill.Discount;


            }

            return Page();
        }

        public IActionResult OnPostCloseBillManually(int HeaderId)
        {

            StatusMessage = _SalesTrans.CloseBillManually(HeaderId).GetAwaiter().GetResult();

            return RedirectToPage("/Sales/Reports/Bills");
        }

        public JsonResult OnGetSearchCustomer(string term)
        {
            if (term == null)
            {
                return new JsonResult("Not Found");
            }
            IQueryable<string> lstCustomers = from P in _db.Customer
                                              where (P.CompanyName.Contains(term))
                                              select P.CompanyName;
            return new JsonResult(lstCustomers);
        }
    }
}
