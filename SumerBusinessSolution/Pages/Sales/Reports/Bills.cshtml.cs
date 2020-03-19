﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace SumerBusinessSolution
{
    [Authorize]
    public class BillsModel : PageModel
    {
 

            private readonly ApplicationDbContext _db;
            private readonly ISalesTrans _SalesTrans;

            //private readonly IServiceScopeFactory _serviceScopeFactory;
            public BillsModel(ApplicationDbContext db, ISalesTrans SalesTrans)
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

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SearchFromDate { get; set; }

    [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime SearchToDate = DateTime.Now;

        public async Task<IActionResult> OnGet(string CustomerName = null, DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
            {
               //  CustomerList = _db.Customer.ToList();
                BillHeaderList = await _db.BillHeader.Include(header=> header.Customer).Where(header => header.CreatedDataTime >= DateTime.Now.AddMonths(-1)).ToListAsync();
             
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


            if (SearchFromDate != null & SearchToDate !=null & CustomerName == null)
            {
                BillHeaderList = await _db.BillHeader.Include(header => header.Customer).Where(u => u.CreatedDataTime >= SearchFromDate & u.CreatedDataTime <= SearchToDate).ToListAsync();
            }
            else
            {
                if (SearchFromDate != null & SearchToDate != null & CustomerName != null)
                {
                    BillHeaderList = await _db.BillHeader.Include(header => header.Customer).Where(u => u.Customer.CompanyName.ToLower().Contains(CustomerName.ToLower()) & u.CreatedDataTime >= SearchFromDate & u.CreatedDataTime <= SearchToDate).ToListAsync();
                }
                else
                {
                    if (SearchFromDate == null & SearchToDate == null & CustomerName != null)
                    {
                        BillHeaderList = await _db.BillHeader.Include(header => header.Customer).Where(u => u.Customer.CompanyName.ToLower().Contains(CustomerName.ToLower())).ToListAsync();

                    }
                    else
                    {
                           BillHeaderList = await _db.BillHeader.Include(header => header.Customer).Where(header => header.CreatedDataTime >= DateTime.Now.AddMonths(-1)).ToListAsync();
                    }
                }
                  
              
               
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
