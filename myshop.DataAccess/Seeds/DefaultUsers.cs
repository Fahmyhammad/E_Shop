using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using myshop.Entities.Models;
using myshop.Utilities;

namespace myshop.DataAccess.Seeds
{
    public static class DefaultUsers
    {

        public static async Task SeedAdminUser(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            try
            {
                if (!userManager.Users.Any())
                {
                    var defaultUser = new AppUser
                    {
                        UserName = "baseadmin@gmail.com",
                        Email = "baseadmin@gmail.com",
                        Name = "Fahmy Hammad",
                        PhoneNumber = "01006747608",
                        Phone = "01006747608",
                        Address = "Tala",
                        EmailConfirmed = true,
                        City = "Tala"
                    };

                    var user = await userManager.FindByEmailAsync(defaultUser.Email);
                    if (user == null)
                    {
                        // تأكد من وجود الدور
                        var roleExists = await roleManager.RoleExistsAsync(SD.AdminRole);
                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole(SD.AdminRole));
                        }

                        // إنشاء المستخدم
                        var result = await userManager.CreateAsync(defaultUser, "Admin690@");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(defaultUser, SD.AdminRole);
                            logger.LogInformation("Admin user created successfully.");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                logger.LogError($"Error creating user: {error.Description}");
                            }
                        }
                    }
                    else
                    {
                        logger.LogWarning("Admin user already exists.");
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"An exception occurred: {ex.Message}");
            }

        }


        public static async Task SeedUserAsync(UserManager<AppUser> userManager, ILogger logger)
        {

            var defaultUser = new AppUser
            {
                Name = "User",
                UserName = "user@gmail.com",
                Email = "user@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "01006747608",
                Phone = "01006747608",
                Address = "Tala",
                City = "Tala"
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                var result = await userManager.CreateAsync(defaultUser, "User690@");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, SD.CustomerRole);
                }
                else
                {
                    logger.LogError("Error creating user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

        }

    }
}
