using Serilog;

namespace Nexus.Authentication.WebAPI.Scope.Extensions
{
    public static class LoggingBuilderExtensions
    {
        public static void AddCustomLogging(this ILoggingBuilder logging)
        {
            var logger = new LoggerConfiguration()
               .WriteTo.Console()
               .Enrich.FromLogContext()
               .CreateLogger();
            logging.ClearProviders();
            logging.AddSerilog(logger);
        }
    }
}
