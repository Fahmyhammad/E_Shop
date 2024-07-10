using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using myshop.Entities.Models;
using myshop.myshop.DataAccess.Data;
using myshop.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;

        public DbInitializer(AppDbContext db,
        UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initializer()
        {
            // Migration
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            // Roles

            if (_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

                // User

                _userManager.CreateAsync(new AppUser
                {
                    UserName = "admin",
                    Email = "baseadmin@gmail.com",
                    Name = "Fahmy Hammad",
                    PhoneNumber = "1234567890",
                    Address = "Tala",
                    City = "Tala"
                }, "Fahmy690@").GetAwaiter().GetResult();

                AppUser user = _db.appUsers.FirstOrDefault(x => x.Email == "baseadmin@gmail.com");
                _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();

            }

            return;

        }
    }

}
