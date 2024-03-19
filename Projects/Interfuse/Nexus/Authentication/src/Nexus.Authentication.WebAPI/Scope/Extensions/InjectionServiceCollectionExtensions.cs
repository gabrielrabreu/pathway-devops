using Nexus.Authentication.WebAPI.Service;
using Nexus.Authentication.WebAPI.Service.Interfaces;

namespace Nexus.Authentication.WebAPI.Scope.Extensions
{
    public static class InjectionServiceCollectionExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
