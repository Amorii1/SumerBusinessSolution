using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Inventory.Products
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {

            _db = db;
        }
        [BindProperty]
        public ProdInfo ProdInfo { get; set; }

        public List<InvTransaction> invTransactionList { get; set; }
        public List<IncomingGood> IncomingGoodList { get; set; }

        //public List<InvStockQty> InvStockQtyList { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            else
            {
                ProdInfo = await _db.ProdInfo
                    .Include(m=> m.ApplicationUser)
                    .FirstOrDefaultAsync(m => m.Id == ID);
                if (ProdInfo == null)
                {

                    return NotFound();
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                invTransactionList = _db.InvTransaction.Where(it => it.ProdId == ProdInfo.Id).ToList();
                _db.InvTransaction.RemoveRange(invTransactionList);

                IncomingGoodList = _db.IncomingGood.Where(it => it.ProdId == ProdInfo.Id).ToList();
                _db.IncomingGood.RemoveRange(IncomingGoodList);
                _db.ProdInfo.Remove(ProdInfo);
                await _db.SaveChangesAsync();
                StatusMessage = "تم حذف المادة";
            }
            catch(Exception ex)
            {
                StatusMessage =  "Error! لا يمكن حذف المادة. لوجود حركات تجارية متعلقة بهذه المادة";
            }
       
            return RedirectToPage("Index");
        }

    }
}