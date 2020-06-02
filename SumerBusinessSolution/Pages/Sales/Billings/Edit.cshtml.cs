using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;
using SumerBusinessSolution.Utility;

namespace SumerBusinessSolution.Pages.Sales.Billings
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ICustomerTrans _CustTrans;

        private readonly ISalesTrans _SalesTrans;
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public EditModel(ApplicationDbContext db, ISalesTrans SalesTrans, ICustomerTrans CustTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
            _CustTrans = CustTrans;
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
        public string SelectedWh { get; set; }

        [BindProperty]
        public int WhId { get; set; }

        [BindProperty]
        public IList<Customer> CustomerList { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }

        //[BindProperty]
        //public IList<Customer> CustomerList { get; set; }

        public List<BillItems> Bi { get; set; }


        public List<PricingType> UnitPriceTypesList { get; set; }

        [Display(Name = "الية الدفع")]
        public List<string> PaymentTermsList = new List<string>();

        [BindProperty]
        public PricingType PricingType { get; set; }

        [BindProperty]
        public string COD { get; set; }

        [BindProperty]
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }

        public InvStockQty InvStockQty { get; set; }
        public ActionResult OnGet(int BhId)
        {
            try
            {


                COD = SD.COD;
                Bi = _db.BillItems
               .Include(bill => bill.BillHeader)
               .Include(bill => bill.BillHeader.Customer)
               .Include(bill => bill.ProdInfo)
               .Include(bill => bill.BillHeader.ApplicationUser)
               .Where(bill => bill.HeaderId == BhId).ToList();

                BillItems BillItem = Bi[0];

                BillHeader = BillItem.BillHeader;

                CustomerName = BillItem.BillHeader.Customer.CompanyName;
                CustomerId = BillItem.BillHeader.Customer.Id;
                Warehouse Wh = _db.Warehouse.FirstOrDefault(wh => wh.Id == BillItem.WhId);
                SelectedWh = Wh.WhName;
                WhId = Wh.Id;
                // Bi = new List<BillItems> { new BillItems { ProdId = 0, Qty = 0, UnitPrice = 0, TotalAmt = 0, Note = "" } };

                UnitPriceTypesList = _db.PricingType.ToList();

                PaymentTermsList.Add(SD.COD);
                PaymentTermsList.Add(SD.Postpaid);
            }
            catch
            {

            }
            return Page();
        }
        public ActionResult OnPost(List<BillItems> Bi, int CustomerId, int WhId)
        {
            BillHeader.CustId = CustomerId;
            int BhId = BillHeader.Id;

            // creating new bill (will create a new bill similar to the older one, after that the old one will be deleted)
            StatusMessage = _SalesTrans.CreateBill(BillHeader, Bi, WhId, "Edit", BhId).GetAwaiter().GetResult();

 
            ModelState.Clear();

            if (BillHeader.Id != 0)
            {
                return RedirectToPage("/Sales/Billings/PrintBill", new { BhId = BillHeader.Id });

            }
            else
            {
                return RedirectToPage("/Sales/Billings/Create");
            }
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

            if (InvStockQty == null)
            {
                return false;
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


    }

}