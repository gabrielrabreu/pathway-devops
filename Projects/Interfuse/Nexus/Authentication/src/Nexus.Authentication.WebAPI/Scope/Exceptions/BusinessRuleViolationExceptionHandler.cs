using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nexus.Authentication.WebAPI.Exceptions;
using System.Net;

namespace Nexus.Authentication.WebAPI.Scope.Exceptions
{
    public class BusinessRuleViolationExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<BusinessRuleViolationExceptionHandler> _logger;

        public BusinessRuleViolationExceptionHandler(ILogger<BusinessRuleViolationExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                                    Exception exception,
                                                    CancellationToken cancellationToken)
        {
            if (exception is BusinessRuleViolationException)
            {
                _logger.LogError(exception, "An business rule violation error occurred");

                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.UnprocessableEntity,
                    Type = exception.GetType().Name,
                    Title = "An business rule violation error occurred",
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
