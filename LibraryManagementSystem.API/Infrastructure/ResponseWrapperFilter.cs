using Microsoft.AspNetCore.Http;
using LibraryManagementSystem.Application.Common.Wrappers;
using System.Threading.Tasks;

namespace LibraryManagementSystem.API.Infrastructure
{
    public class ResponseWrapperFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var result = await next(context);

            if (result is null)
            {
                return Results.NotFound(new ApiResponse<object>("Resource not found"));
            }

            // If the endpoint returns an IResult (e.g., Results.NotFound(), Results.BadRequest()),
            // we assume the endpoint has already formatted the response or it's a specific control flow.
            // Ideally, we should also wrap wrapper-less IResults, but for now, let's focus on successful raw data returns.
            if (result is IResult)
            {
                 if (result is IValueHttpResult valueResult && 
                     result is IStatusCodeHttpResult statusCodeResult && 
                     statusCodeResult.StatusCode >= 200 && statusCodeResult.StatusCode < 300)
                 {
                     return Results.Ok(new ApiResponse<object>(valueResult.Value!, "Request processed successfully"));
                 }
                 
                return result;
            }

            // If it's a raw object (e.g., AuthorDto, int), wrap it in success response
            return Results.Ok(new ApiResponse<object>(result, "Request processed successfully"));
        }
    }
}
