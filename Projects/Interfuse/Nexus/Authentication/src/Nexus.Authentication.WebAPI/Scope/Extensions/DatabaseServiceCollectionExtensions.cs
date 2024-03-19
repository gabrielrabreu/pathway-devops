using Microsoft.EntityFrameworkCore;
using Nexus.Authentication.WebAPI.Data;

namespace Nexus.Authentication.WebAPI.Scope.Extensions
{
    public static class DatabaseServiceCollectionExtensions
    {
        public static void AddCustomDatabase(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("name=ConnectionStrings:Sqlite"));
        }
    }
}
