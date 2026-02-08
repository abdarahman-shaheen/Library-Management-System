using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Application.Common.Dtos;
using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Features.Loans.Queries;
using LibraryManagementSystem.Application.Features.Loans.Commands;

namespace LibraryManagementSystem.API.Endpoints
{
    public static class LoanEndpoints
    {
        public static void MapLoanEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/loans").WithTags("Loans").RequireAuthorization();

            group.MapGet("/", async (ISender sender) =>
            {
                 var loans = await sender.Send(new GetLoansMainQuery());
                return Results.Ok(new ApiResponse<object>(loans, "Loans retrieved successfully"));
            });

            group.MapGet("/with-details", async (ISender sender) =>
            {
                var loans = await sender.Send(new GetLoansQuery());
                return Results.Ok(new ApiResponse<IEnumerable<LoanDto>>(loans, "Loans retrieved successfully"));
            });

            group.MapPost("/", async ([FromBody] CreateLoanCommand command, ISender sender) =>
            {
                var id = await sender.Send(command);
                return Results.Ok(new ApiResponse<int>(id, "Loan created successfully"));
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateLoanCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest(new ApiResponse<string>("ID mismatch"));
                await sender.Send(command);
                return Results.Ok(new ApiResponse<string>("Loan updated successfully", ""));
            });
        }
    }
}
