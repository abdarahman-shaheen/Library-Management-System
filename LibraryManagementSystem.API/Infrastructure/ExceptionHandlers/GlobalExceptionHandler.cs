using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using LibraryManagementSystem.Application.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.API.Infrastructure.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);

            var response = new ApiResponse<string>("An internal server error has occurred.")
            {
                Succeeded = false,
                Errors = new List<string> { exception.Message } 
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
