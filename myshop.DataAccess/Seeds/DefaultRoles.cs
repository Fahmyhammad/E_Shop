using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using myshop.Utilities;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace myshop.DataAccess.Seeds
{

    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<string> { SD.AdminRole, SD.CustomerRole };
                foreach (var roleName in roles)
                {
                    var role = new IdentityRole { Name = roleName };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        logger.LogError("Error creating role {Role}: {Errors}", roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
    }
}