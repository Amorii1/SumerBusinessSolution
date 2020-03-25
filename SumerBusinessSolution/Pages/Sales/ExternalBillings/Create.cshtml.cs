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

namespace SumerBusinessSolution.Pages.Sales.ExternalBillings
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        private readonly ISalesTrans _SalesTrans;
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public CreateModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ExternalBillHeader ExternalBillHeader { get; set; }

        [BindProperty]
        public ExternalBillItems ExternalBill { get; set; }

        [BindProperty]
        public string ProdCode { get; set; }

        [BindProperty]
        public IList<Warehouse> WarehouseList { get; set; }

        [BindProperty]
        public IList<Customer> Customer { get; set; }

        public List<ExternalBillItems> Bi { get; set; }


       // public List<PricingType> UnitPriceTypesList { get; set; }

       // [BindProperty]
      //  public PricingType PricingType { get; set; }

        [BindProperty]
        public string Selected { get; set; }



       // public InvStockQty InvStockQty { get; set; }
        public ActionResult OnGet()
        {
            Bi = new List<ExternalBillItems> { new ExternalBillItems { ProdName = null, Qty = 0, UnitPrice = 0, TotalAmt = 0, Note = "" } };
            
            WarehouseList = _db.Warehouse.ToList();
            Customer = _db.Customer.Where(cus => cus.Status == SD.ActiveCustomer).ToList();


            return Page();
        }
        public ActionResult OnPost(List<ExternalBillItems> Bi)
        {

            StatusMessage = _SalesTrans.CreateExternalBill(ExternalBillHeader, Bi).GetAwaiter().GetResult();

            ModelState.Clear();

            return RedirectToPage("/Sales/ExternalBillings/Create");
        }

        
 
    }

}
 
