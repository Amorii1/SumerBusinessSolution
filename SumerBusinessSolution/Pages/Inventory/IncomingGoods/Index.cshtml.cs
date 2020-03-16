using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Inventory.IncomingGoods
{
    [Authorize]

    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public IEnumerable<IncomingGood> IncomingGoodList { get; set; }
        public IncomingGood IncomingGoods { get; set; }
        public ProdInfo ProdInfo { get; set; }
        public Warehouse Warehouse { get; set; }


        public IActionResult OnGet(string searchCreateDateTime = null, string searchProdCode = null)
        {
            IncomingGoodList =  _db.IncomingGood.Include(tr => tr.Warehouse).Include(tr => tr.ProdInfo).Include(tr=> tr.ApplicationUser)
               .Where(tr => tr.CreatedDateTime > DateTime.Now.AddMonths(-2)).ToList().OrderBy(tr => tr.CreatedDateTime); ;
          


            StringBuilder Param = new StringBuilder();

            Param.Append("&searchProdCode=");

            if (searchProdCode != null)
            {
                Param.Append(searchProdCode);
            }

            Param.Append("&searchCreateDateTime=");

            if (searchCreateDateTime != null)
            {
                Param.Append(searchCreateDateTime);
            }

            if (searchProdCode != null)
            {
                IncomingGoodList = _db.IncomingGood.Where(u => u.ProdInfo.ProdCode.ToLower().Contains(searchProdCode.ToLower())).ToList();

            }
            else
            {
                if (searchCreateDateTime != null)
                {

                    IncomingGoodList = _db.IncomingGood.Where(u => u.CreatedDateTime.ToString().Contains(searchCreateDateTime)).ToList();

                }
            }

            return Page();
        }

    }
}