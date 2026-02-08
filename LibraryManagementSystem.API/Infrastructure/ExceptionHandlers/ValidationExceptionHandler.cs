using Microsoft.AspNetCore.Diagnostics;
using LibraryManagementSystem.Application.Validators.Exceptions;

namespace LibraryManagementSystem.API.Infrastructure.ExceptionHandlers
{
    public class ValidationExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public ValidationExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ValidationException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    Exception = exception,
                    ProblemDetails =
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation Error",
                        Detail = "One or more validation failures have occurred.",
                        Extensions = 
                        {
                            ["succeeded"] = false,
                            ["errors"] = validationException.Errors
                        }
                    }
                });
            }

            return false;
        }
    }
}
