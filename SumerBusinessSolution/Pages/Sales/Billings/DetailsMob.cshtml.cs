using System;
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
using System.IO;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Net;
using SelectPdf;

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
                .Include(bill => bill.BillHeader)
                .Include(bill => bill.BillHeader.Customer)
                .Include(bill => bill.ProdInfo)
                .Include(bill => bill.BillHeader.ApplicationUser)
                .Where(bill => bill.HeaderId == BhId).ToListAsync();
            if (BillItemsList.Count() > 0)
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

        // Here's The Print Bill Function

        public IActionResult OnGetPrintBill(int BhId)
        {
            string path = Request.Host.Value;
            if (BhId != 0)
            {
 
                var body = RazorPage.RenderToString("https://" + path + "/Sales/Billings/InvoicePrint?BhId=" + BhId);

                var converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                converter.Options.WebPageWidth = 1024;
                converter.Options.WebPageHeight = 0;
                converter.Options.WebPageFixedSize = false;

                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;

                //converter.Options.PdfPageCustomSize = new System.Drawing.SizeF(816, 1020);
                PdfDocument doc = converter.ConvertHtmlString(body, "https://" + path + "/Sales/Billings/InvoicePrint?BhId=" + BhId);
                byte[] pdf = doc.Save();
                doc.Close();
                string BillName = "فاتورة" + "-" + BhId + ".pdf";
                return new FileContentResult(pdf, "application/pdf")
                {
                    
                    FileDownloadName = BillName 
                    
                };
 

            }
            else
            {
                return Page();
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