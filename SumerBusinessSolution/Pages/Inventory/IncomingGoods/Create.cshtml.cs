﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sumer.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Pages.Inventory.IncomingGoods
{
   [Authorize(Roles = SD.AdminEndUser)]

   [Authorize(Roles = SD.SupervisorEndUser)]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IInventoryTrans _InveTrans;
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public CreateModel(ApplicationDbContext db, IInventoryTrans InveTrans)
        {
            _db = db;
            _InveTrans = InveTrans;
 
        }

        [BindProperty]
        public IncomingGood IncomingGood { get; set; }
        public IList<IncomingGood> IncomingGoodlist { get; set; }
        public IList<ProdInfo> ProdInfo { get; set; }
        public Warehouse Warehouse { get; set; }

        [BindProperty]
        public IList<Warehouse> WarehouseList { get; set; }

        [BindProperty]
        public int WhId { get; set; }

        [BindProperty]
        public int ProdId { get; set; }

        [Required]
        [BindProperty]
       [Display(Name ="الكمية")]
        public double Qty { get; set; }

        [BindProperty]
       [Display(Name = "الملاحظات")]

        public string Note { get; set; }

        [Required]
        [BindProperty]
      //  [Display(Name = "رمز المنتج")]

        public string ProdCode { get; set; }

        [TempData]
        public string StatusMessage { get; set; }


        public void OnGet()
        {
           WarehouseList = _db.Warehouse.ToList();
 
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

        public async Task<IActionResult> OnPost()
        {
            int ProdId;
            try
            {
                ProdId = _db.ProdInfo.FirstOrDefault(pro => pro.ProdCode == ProdCode).Id;
            }
            catch
            {
                StatusMessage = ("Error! Product code can not be found");
                return RedirectToPage("/inventory/incominggoods/create");
            }

            bool incomingoods = true; //_InveTrans.CreateIncomingGoods(WhId, ProdId, Qty, Note).GetAwaiter().GetResult();

            if (incomingoods == true)
            {
               // StatusMessage = "New goods have been added successfully.";
                StatusMessage = "تمت الاضافة";

            }
            else
            {
                //StatusMessage = "Error! New goods havenot been added successfully.";
                StatusMessage = "عذرا!! لم تتم اضافة البضاعة الواردة بنجاح";
            
            }

            return RedirectToPage("/inventory/incominggoods/create");
        }
    }
}
   