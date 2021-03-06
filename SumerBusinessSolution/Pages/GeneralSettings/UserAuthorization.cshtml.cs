﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SumerBusinessSolution.Utility;
using SumerBusinessSolution.Data;
using SumerBusinessSolution.Models;

namespace SumerBusinessSolution
{
    public class UserAuthorizationModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public UserAuthorizationModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public RoleAuth RoleAuth { get; set; }

        [BindProperty]
        public bool SuperAppTransReq { get; set; }

        [BindProperty]
        public bool SuperCreateIG { get; set; }


        public IActionResult OnGet()
        {
            RoleAuth = _db.RoleAuth.FirstOrDefault(ro => ro.RoleName == SD.SupervisorUser);
            if(RoleAuth != null)
            {
                SuperAppTransReq = RoleAuth.AppTransReq;
                SuperCreateIG = RoleAuth.CreateInGoods;
            }
           
            return Page();

        }

        public async Task<IActionResult> OnPost()
        {
            RoleAuth = _db.RoleAuth.FirstOrDefault(ro => ro.RoleName == SD.SupervisorUser);
            if(RoleAuth != null)
            {
                RoleAuth.AppTransReq = SuperAppTransReq;
                RoleAuth.CreateInGoods = SuperCreateIG;
            }
       


            await _db.SaveChangesAsync();
            StatusMessage = "تم التعديل بنجاح";

            return RedirectToPage("/GeneralSettings/UserAuthorization");
        }
    }
}