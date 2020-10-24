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
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Net;
using SelectPdf;

namespace SumerBusinessSolution.Pages.Sales.ExternalBillings
{
    [Authorize]
    public class DetailsModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        private readonly ISalesTrans _SalesTrans;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public DetailsModel(ApplicationDbContext db, ISalesTrans SalesTrans)
        {
            _db = db;
            _SalesTrans = SalesTrans;
        }
        [BindProperty]
        public ExternalBillHeader ExternalBillHeader { get; set; }

        [BindProperty]
        public  ExternalBillItems ExternalBillItems { get; set; }
        [BindProperty]


        public List<ExternalBillItems> ExternalBillItemsList { get; set; }

        [BindProperty]
        public CompanyInfo CompanyInfo { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<ActionResult> OnGet(int BhId)
        {

              ExternalBillItemsList = await _db.ExternalBillItems
             .Include(bill => bill.ExternalBillHeader)
             .Include(bill => bill.ExternalBillHeader.Customer)
             .Include(bill => bill.ExternalBillHeader.ApplicationUser)
             .Include(bill => bill.ProdInfo)
             .Where(bill => bill.ExternalBillHeader.Id == BhId).ToListAsync();
            if (ExternalBillItemsList.Count() > 0)
            {
                ExternalBillHeader = ExternalBillItemsList[0].ExternalBillHeader;
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

        public ActionResult OnPost(ExternalBillHeader ExternalBillHeader)
        {
            string path = Request.Host.Value;
            if (ExternalBillHeader.Id != 0)
            {
                //return RedirectToPage("/Sales/Billings/PrintBill", new { BhId = ExternalBillHeader.Id });

                var body = RazorPage.RenderToString("http://" + path + "/Sales/ExternalBillings/InvoicePrint?BhId=" + ExternalBillHeader.Id);

                var converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.WebPageWidth = 1024;
                converter.Options.WebPageHeight = 0;
                converter.Options.WebPageFixedSize = false;

                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                // converter.Options.PdfPageCustomSize = new System.Drawing.SizeF(816, 1020);
                PdfDocument doc = converter.ConvertHtmlString(body, "http://" + path + "/Sales/ExternalBillings/InvoicePrint?BhId=" + ExternalBillHeader.Id);

                byte[] pdf = doc.Save();
                doc.Close();

                return new FileContentResult(pdf, "application/pdf")
                {
                    FileDownloadName =  ExternalBillHeader.Id + "-" + "فاتورة.pdf"

                };
            }
            return RedirectToPage("/Sales/ExternalBillings/Details", new { BhId = ExternalBillHeader.Id });
        }

        public ActionResult OnGetPdfDownload(int id)
        {
            string path = Request.Host.Value;
            //if (id != 0)
            //{
            //return RedirectToPage("/Sales/Billings/PrintBill", new { BhId = ExternalBillHeader.Id });

            var body = RazorPage.RenderToString("http://" + path + "/Sales/ExternalBillings/InvoicePrint?BhId=" + id);

            var converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.WebPageWidth = 1024;
            converter.Options.WebPageHeight = 0;
            converter.Options.WebPageFixedSize = false;

            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
            // converter.Options.PdfPageCustomSize = new System.Drawing.SizeF(816, 1020);
            PdfDocument doc = converter.ConvertHtmlString(body, "http://" + path + "/Sales/ExternalBillings/InvoicePrint?BhId=" + id);

            byte[] pdf = doc.Save();
            doc.Close();
            return new FileContentResult(pdf, "application/pdf")
            {
                FileDownloadName = "فاتورة.pdf"
            };
            //}
            //return  Page();
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


        public IActionResult OnPostCloseBillManually(int BhId)
        {

            StatusMessage = _SalesTrans.CloseExternalBillManually(BhId).GetAwaiter().GetResult();

            return RedirectToPage("/Sales/ExternalBillings/Index");
        }


        public IActionResult OnPostDeleteBill(int BhId)
        {

            StatusMessage = _SalesTrans.DeleteExternalBill(BhId).GetAwaiter().GetResult();

            return RedirectToPage("/Sales/ExternalBillings/Index");
        }

        public async Task<IActionResult> OnPostEditBill(int BhId)
        {
            ExternalBillItemsList = await _db.ExternalBillItems
           .Include(bill => bill.ExternalBillHeader)
           .Include(bill => bill.ExternalBillHeader.Customer)
           .Include(bill => bill.ProdInfo)
           .Include(bill => bill.ExternalBillHeader.ApplicationUser)
           .Where(bill => bill.HeaderId == BhId).ToListAsync();

            return RedirectToPage("/Sales/ExternalBillings/Edit", new { Bi = ExternalBillItemsList, BhId });
        }


    }
}