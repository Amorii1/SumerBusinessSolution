using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;

namespace SumerBusinessSolution.Pages.Sales.Billings
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
        public BillHeader BillHeader { get; set; }

        [BindProperty]
        public string ProdCode { get; set; }

        [BindProperty]
        public IList<Warehouse> WarehouseList { get; set; }

        [BindProperty]
        [Display(Name = "المخزن")]
        public int SelectedWh { get; set; }

        [BindProperty]
        public IList<Customer> Customer { get; set; }

        public List<BillItems> Bi { get; set; }


        public List<PricingType> UnitPriceTypesList { get; set; }

        [BindProperty]
        public PricingType PricingType { get; set; }

        [BindProperty]
        public string Selected { get; set; }

        [BindProperty]
        public string CustomerName { get; set; }

        public InvStockQty InvStockQty { get; set; }
        public ActionResult OnGet()
        {
            Bi = new List<BillItems> { new BillItems { ProdId = 0, Qty = 0, UnitPrice = 0, TotalAmt = 0, Note = "" } };
            
            WarehouseList = _db.Warehouse.Where(wh=> wh.WhType.Type.ToLower() == SD.ShowRoom.ToLower()).ToList();
            Customer = _db.Customer.Where(cus => cus.Status == SD.ActiveCustomer).ToList();

            UnitPriceTypesList = _db.PricingType.ToList();
 
            //UnitPriceTypesList.Add(SD.WholePrice);
            // UnitPriceTypesList.Add(SD.RetailPrice);

           

            return Page();
        }
        public ActionResult OnPost(List<BillItems> Bi)
        {
            Customer Customer = _db.Customer.FirstOrDefault(c=> c.CompanyName == CustomerName);
            BillHeader.CustId = Customer.Id;
            StatusMessage = _SalesTrans.CreateBill(BillHeader, Bi, SelectedWh).GetAwaiter().GetResult();


            //_db.SaveChanges();

            ModelState.Clear();

            // }
            // }
            return RedirectToPage("/Sales/Billings/Create");
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

            //int x = Bi.Count();

            //Bi[0].UnitPrice =  500;

            return new JsonResult(lstProdCode);

        }

        public JsonResult OnGetProdUnitPriceWhole(string term)
        {
            if (term == null)
            {
                return new JsonResult("Not Found");
            }
 
            IQueryable<double> unitPrice = null;
   
                  unitPrice = from P in _db.ProdInfo
                                               where (P.ProdCode.Contains(term))
                                               select P.WholePrice;
            //}
            
            return new JsonResult(unitPrice);

        }

        public JsonResult OnGetProdUnitPriceRetail(string term)
        {
            if (term == null)
            {
                return new JsonResult("Not Found");
            }

            IQueryable<double> unitPrice = null;

            unitPrice = from P in _db.ProdInfo
                        where (P.ProdCode.Contains(term))
                        select P.RetailPrice;
            //}

            return new JsonResult(unitPrice);

        }

        public JsonResult OnGetCheckQty(string term, double qty)
        {
            if (term == null)
            {
                return new JsonResult("Not Found");
            }
            bool qtyCheck = CheckQtyInWh(term, qty);

            return new JsonResult(qtyCheck);

        }

        // leave it for later
        private bool CheckQtyInWh(string ProdCode, double Qty)
        {
            InvStockQty = _db.InvStockQty.FirstOrDefaultAsync(inv => inv.ProdInfo.ProdCode == ProdCode & inv.Warehouse.WhType.Type == "StoreRoom").GetAwaiter().GetResult();

            double StockQty = InvStockQty.Qty;

            if (StockQty >= Qty)
            {
                return true;
            }
            else
            {
                return false;
            }
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
 
