﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;
using Microsoft.AspNetCore.Localization;
using System.ComponentModel.DataAnnotations;


namespace SumerBusinessSolution.Pages.Sales.Billings
{ 
    public class DetailsMobModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        private readonly ISalesTrans _SalesTrans;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public DetailsMobModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }
        [BindProperty]
        public BillHeader BillHeader { get; set; }

        [BindProperty]
        public BillItems BillItems { get; set; }
        [BindProperty]
 

        public List<BillItems> BillItemsList { get; set; }

        [BindProperty]
        public CompanyInfo CompanyInfo { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<ActionResult> OnGet(int BhId)
        {

            BillItemsList = await _db.BillItems
                .Include(bill=> bill.BillHeader)
                .Include(bill=> bill.BillHeader.Customer)
                .Include(bill=> bill.ProdInfo)
                .Include(bill=> bill.BillHeader.ApplicationUser)
                .Where(bill => bill.HeaderId == BhId).ToListAsync();
            if(BillItemsList.Count() > 0)
            {
                BillHeader = BillItemsList[0].BillHeader;
            }

            try
            {
                CompanyInfo = _db.CompanyInfo.FirstOrDefault();
            }
            catch
            {

            }

            return Page();
        }
        public void OnPost()
        {

        }

        public IActionResult OnPostCloseBillManually(int HeaderId)
        {

            StatusMessage = _SalesTrans.CloseBillManually(HeaderId).GetAwaiter().GetResult();

            return RedirectToPage("/Sales/Billings/Index");
        }

        public IActionResult OnPostDeleteBill(int HeaderId)
        {

            StatusMessage = _SalesTrans.DeleteBill(HeaderId).GetAwaiter().GetResult();

            return RedirectToPage("/Sales/Billings/Index");
        }

        //public async Task<IActionResult> OnPostEditBill(int BhId)
        //{
        //    BillItemsList = await _db.BillItems
        //   .Include(bill => bill.BillHeader)
        //   .Include(bill => bill.BillHeader.Customer)
        //   .Include(bill => bill.ProdInfo)
        //   .Include(bill => bill.BillHeader.ApplicationUser)
        //   .Where(bill => bill.HeaderId == BhId).ToListAsync();
 
        //    return RedirectToPage("/Sales/Billings/Edit", new { Bi = BillItemsList, BhId } );
        //}


    }
}