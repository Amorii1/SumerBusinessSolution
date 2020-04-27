using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Models;
using SumerBusinessSolution.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Data
{
    public class DbInitializar : IDbInitializar
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializar(ApplicationDbContext db,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initializar()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (_roleManager.RoleExistsAsync(SD.AdminUser).GetAwaiter().GetResult())
            {
                return;
            }

            // Creating User Roles
            _roleManager.CreateAsync(new IdentityRole(SD.AdminUser)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.SupervisorUser)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.BnasStoreUser)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.YaseenStoreUser)).GetAwaiter().GetResult();

            // Create Admin User
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "1233456456",
                FirstName = "Bnas admin"
            }, "Qwer!234"
            ).GetAwaiter().GetResult();

            // assign role to admin user
            IdentityUser user = _db.ApplicationUser.FirstOrDefaultAsync(u => u.Email == "admin@gmail.com").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, SD.AdminUser).GetAwaiter().GetResult();

            // Creating Price Types (Retail price and wholesales price)
            PricingType RPrices = new PricingType
            {
                PriceType = SD.RetailPrice
            };

            PricingType WPrices = new PricingType
            {
                PriceType = SD.WholePrice
            };
            _db.PricingType.Add(RPrices);
            _db.PricingType.Add(WPrices);

            // Create Authentication Roles for Supervisor
            RoleAuth Auth = new RoleAuth
            {
                RoleName = SD.SupervisorUser,
                AppTransReq = false,
                CreateInGoods = false
            };
            _db.RoleAuth.Add(Auth);

            // Create Wh types (showrooms and store rooms)
            WhType ShowRoom = new WhType
            {
                Type = SD.ShowRoom
            };

            WhType StoreRoom = new WhType
            {
                Type = SD.StoreRoom
            };
            _db.WhType.Add(ShowRoom);
            _db.WhType.Add(StoreRoom);

            // save all changes
            _db.SaveChangesAsync();
        }
    }
}
