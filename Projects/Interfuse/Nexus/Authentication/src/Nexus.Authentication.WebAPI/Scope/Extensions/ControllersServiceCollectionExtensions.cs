namespace Nexus.Authentication.WebAPI.Scope.Extensions
{
    public static class ControllersServiceCollectionExtensions
    {
        public static void AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers();
        }

        public static void UseCustomControllers(this IEndpointRouteBuilder app)
        {
            app.MapControllers();
        }
    }
}
