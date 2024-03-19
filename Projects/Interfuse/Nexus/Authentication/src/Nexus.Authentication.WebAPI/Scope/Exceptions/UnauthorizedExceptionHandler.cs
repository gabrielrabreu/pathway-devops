using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Nexus.Authentication.WebAPI.Scope.Exceptions
{
    public class UnauthorizedExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<UnauthorizedExceptionHandler> _logger;

        public UnauthorizedExceptionHandler(ILogger<UnauthorizedExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                                    Exception exception,
                                                    CancellationToken cancellationToken)
        {
            if (exception is UnauthorizedAccessException)
            {
                _logger.LogError(exception, "An unauthorized error occurred");

                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Type = exception.GetType().Name,
                    Title = "An unauthorized error occurred",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                };

                httpContext.Response.StatusCode = problemDetails.Status.Value;

                await httpContext.Response
                    .WriteAsJsonAsync(problemDetails, cancellationToken);

                return true;
            }

            return false;
        }
    }
}
