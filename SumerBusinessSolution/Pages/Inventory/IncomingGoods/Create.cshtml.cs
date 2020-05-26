using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Transactions;
using SumerBusinessSolution.Utility;

namespace SumerBusinessSolution.Pages.Inventory.IncomingGoods
{
    
   //[Authorize(Roles = SD.AdminUser)]

   [Authorize]
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
        public IList<ProdInfo> ProdInfoList { get; set; }

        [BindProperty]
        public ProdInfo ProdInfo { get; set; }

        public Warehouse Warehouse { get; set; }

        [BindProperty]
        public IList<Warehouse> WarehouseList { get; set; }

        [BindProperty]
        [Display(Name = "المخزن")]
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

        public List<IncomingGood> InG { get; set; }

        public ActionResult OnGet()
        {
            InG = new List<IncomingGood> { new IncomingGood { ProdId = 0, WhId = WhId , Qty = 0, Note = "" } };

            WarehouseList = _db.Warehouse.Where(wh=> wh.Active == true).OrderByDescending(wh=> wh.WhType.Type).ToList();


            return Page();

        }

        public ActionResult OnPost(List<IncomingGood> InG)
        {

            StatusMessage =  _InveTrans.CreateIncomingGoods(WhId, InG).GetAwaiter().GetResult();

            ModelState.Clear();

            return RedirectToPage("/inventory/incominggoods/create");
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

        public JsonResult Test()
        {
            string ok = "OK!";
            return new JsonResult(ok);
        }

        public ActionResult CreateProduct()
        {
            bool check = _InveTrans.CheckProdCodeExist(ProdInfo.ProdCode).GetAwaiter().GetResult();

            if (check == false)
            {
                StatusMessage = "Error!رمز المادة مضافة سابقا";
                
            }

            var ClaimId = (ClaimsIdentity)User.Identity;
            var Claim = ClaimId.FindFirst(ClaimTypes.NameIdentifier);
            string UserId = Claim.Value;
  

            ProdInfo.CreatedDateTime = DateTime.Now;
            var NewProd = new ProdInfo
            {
                CreatedById = UserId,
                ProdCode = ProdInfo.ProdCode,
                ProdDescription = ProdInfo.ProdDescription,
                ProdName = ProdInfo.ProdName,
                ProdCategory = ProdInfo.ProdCategory,
                CostPrice = ProdInfo.CostPrice,
                RetailPrice = ProdInfo.RetailPrice,
                WholePrice = ProdInfo.WholePrice,
                CreatedDateTime = ProdInfo.CreatedDateTime
            };
 
            _db.ProdInfo.Add(NewProd);

            _db.SaveChanges();
            var Prod = _db.ProdInfo.FirstOrDefault(pr => pr.ProdCode == ProdInfo.ProdCode);
            bool CreateProdInWh = _InveTrans.CreateProdInWh(Prod.Id, 0, 0);
            StatusMessage = "تمت اضافة مادة جديدة";
            // return new JsonResult(StatusMessage);
            //return RedirectToPage("/Inventory/IncomingGoods/Create");
            return Page();
        }
    }
}
   