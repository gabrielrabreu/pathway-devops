using Microsoft.AspNetCore.Identity;
using Nexus.Authentication.WebAPI.Data.Entities;
using Nexus.Authentication.WebAPI.Data.Seed;

namespace Nexus.Authentication.WebAPI.Data
{
    public static class DatabaseInitializer
    {
        public static async Task Migrate(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
        }

        public static async Task Seed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            await IdentitySeed.Seed(userManager, roleManager, configuration);
        }
    }
}
