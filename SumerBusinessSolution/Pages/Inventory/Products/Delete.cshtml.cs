﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution.Pages.Inventory.Products
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {

            _db = db;
        }
        [BindProperty]
        public ProdInfo ProdInfo { get; set; }
        public async Task<IActionResult> OnGet(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            else
            {
                ProdInfo = await _db.ProdInfo.FirstOrDefaultAsync(m => m.Id == ID);
                if (ProdInfo == null)
                {

                    return NotFound();
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPost()
        {
            _db.ProdInfo.Remove(ProdInfo);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }

    }
}