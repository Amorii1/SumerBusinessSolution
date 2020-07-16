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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Net;
using SelectPdf;

namespace SumerBusinessSolution.Pages.Sales.Billings
{
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
        public BillHeader BillHeader { get; set; }

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

        //[BindProperty]
        //public IList<Customer> CustomerList { get; set; }

        public List<BillItems> Bi { get; set; }


        public List<PricingType> UnitPriceTypesList { get; set; }

        public List<InvStockQty> InvStockQtyList { get; set; }

        [Display(Name = "الية الدفع")]
        public List<string> PaymentTermsList = new List<string>();

        [BindProperty]
        public PricingType PricingType { get; set; }

        [BindProperty]
        public string COD { get; set; }

        [BindProperty]
        public string CustomerName { get; set; }

        public InvStockQty InvStockQty { get; set; }
        public ActionResult OnGet()
        {
            COD = SD.COD;
            Bi = new List<BillItems> { new BillItems { ProdId = 0, Qty = 0, UnitPrice = 0, TotalAmt = 0, Note = "" } };

            WarehouseList = _db.Warehouse.Where(wh => wh.WhType.Type.ToLower() == SD.ShowRoom.ToLower()).ToList();
            CustomerList = _db.Customer.Where(cus => cus.Status == SD.ActiveCustomer).ToList();

            UnitPriceTypesList = _db.PricingType.ToList();
            // List<string> PaymentTermsList = new List<string>();

            PaymentTermsList.Add(SD.COD);
            PaymentTermsList.Add(SD.Postpaid);
            //List<InvStockQty> InvStockQtyList;
            InvStockQtyList = _db.InvStockQty
               .Include(inv => inv.Warehouse)
               .Include(inv => inv.ProdInfo).ToList();
            //.Where(inv => inv.ProdInfo.ProdCode == "0002").ToList();

            return Page();
        }
        public ActionResult OnPost(List<BillItems> Bi)
        {
            Customer Customer = _db.Customer.FirstOrDefault(c => c.CompanyName == CustomerName);
            BillHeader.CustId = Customer.Id;
            StatusMessage = _SalesTrans.CreateBill(BillHeader, Bi, SelectedWh, "New", null).GetAwaiter().GetResult();
            //_db.SaveChanges();

            ModelState.Clear();
            return RedirectToPage("/Sales/Billings/Details", new { BhId = BillHeader.Id });
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
        public JsonResult OnGetSearchProdId(string term)
        {
            if (term == null)
            {
                return new JsonResult("Not Found");
            }
            IQueryable<int> lstProdId = from P in _db.ProdInfo
                                        where (P.ProdCode.Contains(term))
                                        select P.Id;
            //ViewData["hdnProdCode"] = term;
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
        public JsonResult OnGetInvStkQty(string term)
        {
            InvStockQtyList = _db.InvStockQty
                .Include(inv => inv.Warehouse)
                .Include(inv => inv.ProdInfo)
             .Where(inv => inv.ProdInfo.ProdCode == term).ToList();
            //ViewData["hdnProdCode"] = term;
            return new JsonResult(InvStockQtyList);
        }
        public JsonResult OnGetStockList(string term)
        {
            InvStockQtyList = _db.InvStockQty
                .Include(inv => inv.Warehouse)
                .Include(inv => inv.ProdInfo)
             .Where(inv => inv.ProdInfo.ProdCode == term).ToList();
           // ViewData["hdnProdCode"] = term;
            return new JsonResult(InvStockQtyList);
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

            return RedirectToPage("/Sales/Billings/Create");
        }

    }
    public static class RazorPage
    {
        public static string RenderToString(string url)
        {
            try
            {
                //Grab page
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                string html = String.Empty;
                using (StreamReader sr = new StreamReader(data))
                {
                    html = sr.ReadToEnd();
                }
                return html;
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }
    }

}

