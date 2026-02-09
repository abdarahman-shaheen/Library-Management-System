using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace LibraryManagementSystem.API.Middleware.ExceptionHandlers
{
    public class BadRequestExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public BadRequestExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is BadHttpRequestException badRequestException)
            {
                int statusCode = badRequestException.StatusCode;
                httpContext.Response.StatusCode = statusCode;

                string detail = badRequestException.Message;
                var errors = new List<string> { detail };

                // If it's a JSON deserialization error, extract the specific field/path
                if (badRequestException.InnerException is JsonException jsonException)
                {
                    detail = "JSON Deserialization Error";
                    errors.Clear();
                    errors.Add(jsonException.Message); // This usually contains the Path: $.Field
                    
                    // If we want even more explicit field info:
                    if (!string.IsNullOrEmpty(jsonException.Path))
                    {
                        errors.Add($"Problematic field: {jsonException.Path}");
                    }
                }

                return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    Exception = exception,
                    ProblemDetails =
                    {
                        Status = statusCode,
                        Title = "Bad Request",
                        Detail = detail,
                        Extensions = 
                        {
                            ["succeeded"] = false,
                            ["errors"] = errors
                        }
                    }
                });
            }

            return false;
        }
    }
}
