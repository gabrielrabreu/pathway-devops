using Nexus.Authentication.WebAPI.Scope.Exceptions;

namespace Nexus.Authentication.WebAPI.Scope.Extensions
{
    public static class ExceptionServiceCollectionExtensions
    {
        public static void AddCustomExceptions(this IServiceCollection services)
        {
            services.AddExceptionHandler<UnauthorizedExceptionHandler>();
            services.AddExceptionHandler<BusinessRuleViolationExceptionHandler>();
            services.AddExceptionHandler<DefaultExceptionHandler>();
        }

        public static void UseCustomExceptions(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(_ => { });
        }
    }
}
