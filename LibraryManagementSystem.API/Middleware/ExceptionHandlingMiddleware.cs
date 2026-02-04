using LibraryManagementSystem.Application.Common.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using LibraryManagementSystem.Application.Validators.Exceptions; // Add this using

namespace LibraryManagementSystem.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred.");



            var response = new ApiResponse<string>(exception.Message);
            response.Succeeded = false;

            if (exception is ValidationException validationEx)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Validation Errors";
                response.Errors = validationEx.Errors;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Internal Server Error";
                // In production, simpler message
                response.Errors = new List<string> { exception.Message }; 
            }

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
