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
using Microsoft.AspNetCore.Authorization;

namespace SumerBusinessSolution.Pages.Sales.ExternalBillings
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ICustomerTrans _CustTrans;


        private readonly ISalesTrans _SalesTrans;
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public CreateModel(ApplicationDbContext db, ISalesTrans SalesTrans, ICustomerTrans CustTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
            _CustTrans = CustTrans;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ExternalBillHeader ExternalBillHeader { get; set; }

        [BindProperty]
        public string ProdCode { get; set; }

        [BindProperty]
        public IList<Warehouse> WarehouseList { get; set; }

        [BindProperty]
        [Display(Name = "المخزن")]
        public int SelectedWh { get; set; }

        [BindProperty]
        public IList<Customer> CustomerList { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }

        public List<ExternalBillItems> Bi { get; set; }


        public List<PricingType> UnitPriceTypesList { get; set; }

        [BindProperty]
        public PricingType PricingType { get; set; }

        [BindProperty]
        public string COD { get; set; }

        [BindProperty]
        public string CustomerName { get; set; }

        public InvStockQty InvStockQty { get; set; }

        [Display(Name = "الية الدفع")]
        public List<string> PaymentTermsList = new List<string>();
        public ActionResult OnGet()
        {
            COD = SD.COD;
            Bi = new List<ExternalBillItems> { new ExternalBillItems { Qty = 0, UnitPrice = 0, TotalAmt = 0, Note = "", IsExternal= false, ProdId = 0} };

            WarehouseList = _db.Warehouse.Where(wh => wh.WhType.Type.ToLower() == SD.ShowRoom.ToLower()).ToList();
            CustomerList = _db.Customer.Where(cus => cus.Status == SD.ActiveCustomer).ToList();

            UnitPriceTypesList = _db.PricingType.ToList();
            PaymentTermsList.Add(SD.COD);
            PaymentTermsList.Add(SD.Postpaid);
            //UnitPriceTypesList.Add(SD.WholePrice);
            // UnitPriceTypesList.Add(SD.RetailPrice);

            return Page();
        }
        public ActionResult OnPost(List<ExternalBillItems> Bi)
        {
            Customer Customer = _db.Customer.FirstOrDefault(c => c.CompanyName == CustomerName);
            ExternalBillHeader.CustId = Customer.Id;
            StatusMessage = _SalesTrans.CreateExternalBill(ExternalBillHeader, Bi, SelectedWh, "New", null).GetAwaiter().GetResult();
 
            ModelState.Clear();

             return RedirectToPage("/Sales/ExternalBillings/PrintExternalBill", new { BhId = ExternalBillHeader.Id });
           // return Page();
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

        public JsonResult OnGetSearchProdId(string term)
        {
            if (term == null)
            {
                return new JsonResult("Not Found");
            }
            IQueryable<int> lstProdId = from P in _db.ProdInfo
                                             where (P.ProdCode.Contains(term))
                                             select P.Id;
            return new JsonResult(lstProdId);
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

        public JsonResult OnGetCheckProdQty(string qty, string prod)
            {
            //double qty = 5909;

            double dqty = Convert.ToDouble(qty);
            if (prod == null)
            {
                return new JsonResult("Not Found");
            }
            bool qtyCheck = CheckQtyInWh(prod, dqty);

            return new JsonResult(qtyCheck);

        }

        // leave it for later
        private bool CheckQtyInWh(string ProdCode, double Qty)
        {
            InvStockQty = _db.InvStockQty.FirstOrDefaultAsync(inv => inv.ProdInfo.ProdCode == ProdCode & inv.Warehouse.WhType.Type == SD.ShowRoom).GetAwaiter().GetResult();

            if(InvStockQty == null)
            {
                return true;
            }

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

        public IActionResult OnPostCreateCustomer()
        {
            try
            {
                StatusMessage = _CustTrans.CreateCustomer(Customer).GetAwaiter().GetResult();
            }
            catch
            {

            }
            return RedirectToPage("/Sales/ExternalBillings/Create");
        }

    }


}

