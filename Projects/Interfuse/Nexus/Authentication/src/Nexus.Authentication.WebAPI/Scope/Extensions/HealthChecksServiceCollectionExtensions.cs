using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Nexus.Authentication.WebAPI.Scope.Extensions
{
    public static class HealthChecksServiceCollectionExtensions
    {
        public static void AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:Sqlite"];
            if (connectionString == null) throw new ArgumentNullException(connectionString);

            services.AddHealthChecks()
                .AddSqlite(connectionString, healthQuery: "select 1", name: "SQL Server", failureStatus: HealthStatus.Unhealthy, tags: ["Feedback", "Database"]);
        }

        public static void UseCustomHealthChecks(this IEndpointRouteBuilder app)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}
