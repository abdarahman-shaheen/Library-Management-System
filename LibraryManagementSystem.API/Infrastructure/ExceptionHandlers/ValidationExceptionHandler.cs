using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Validators.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagementSystem.API.Infrastructure.ExceptionHandlers
{
    public class ValidationExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ValidationException validationException)
            {
                var response = new ApiResponse<string>("Validation Errors", validationException.Errors)
                {
                    Succeeded = false
                };

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                return true;
            }

            return false;
        }
    }
}
