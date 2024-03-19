using Microsoft.AspNetCore.Identity;
using Nexus.Authentication.WebAPI.Data.Entities;
using Nexus.Authentication.WebAPI.Exceptions;

namespace Nexus.Authentication.WebAPI.Data.Seed
{
    public class IdentitySeed
    {
        public static async Task Seed(UserManager<ApplicationUser> userManager,
                                      RoleManager<ApplicationRole> roleManager,
                                      IConfiguration configuration)
        {
            var adminUserName = configuration["AdminUser:UserName"];
            if (adminUserName == null) throw new ArgumentNullException(adminUserName);

            var adminEmail = configuration["AdminUser:Email"];
            if (adminEmail == null) throw new ArgumentNullException(adminEmail);

            var adminPassword = configuration["AdminUser:Password"];
            if (adminPassword == null) throw new ArgumentNullException(adminPassword);

            if (await userManager.FindByNameAsync(adminUserName) != null)
                return;

            ApplicationUser user = new()
            {
                Email = adminEmail,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = adminUserName
            };

            var result = await userManager.CreateAsync(user, adminPassword);

            if (!result.Succeeded)
                throw new BusinessRuleViolationException("Failed to create admin user. Please check user details and try again.");

            await EnsureRoleExists(roleManager, ApplicationRole.Admin);
            await EnsureRoleExists(roleManager, ApplicationRole.User);

            await userManager.AddToRoleAsync(user, ApplicationRole.Admin);
            await userManager.AddToRoleAsync(user, ApplicationRole.User);
        }

        private static async Task EnsureRoleExists(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new ApplicationRole(roleName);
                await roleManager.CreateAsync(role);
            }
        }
    }
}
