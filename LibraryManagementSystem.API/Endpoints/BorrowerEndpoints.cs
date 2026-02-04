using MediatR;
using LibraryManagementSystem.Application.Features.Borrowers.Commands;
using LibraryManagementSystem.Application.Features.Borrowers.Queries;
using Microsoft.AspNetCore.Mvc;

using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Common.Dtos;

namespace LibraryManagementSystem.API.Endpoints
{
    public static class BorrowerEndpoints
    {
        public static void MapBorrowerEndpoints(this IEndpointRouteBuilder app)
        {
             var group = app.MapGroup("/api/borrowers").WithTags("Borrowers").RequireAuthorization();

            group.MapGet("/", async (ISender sender) =>
            {
                var borrowers = await sender.Send(new GetBorrowersQuery());
                return Results.Ok(new ApiResponse<IEnumerable<BorrowerDto>>(borrowers, "Borrowers retrieved successfully"));
            });

            group.MapGet("/{id}", async (int id, ISender sender) =>
            {
                var borrower = await sender.Send(new GetBorrowerByIdQuery { Id = id });
                return borrower != null 
                    ? Results.Ok(new ApiResponse<BorrowerDto>(borrower, "Borrower retrieved successfully")) 
                    : Results.NotFound(new ApiResponse<BorrowerDto>("Borrower not found"));
            });

            group.MapPost("/", async ([FromBody] CreateBorrowerCommand command, ISender sender) =>
            {
                var id = await sender.Send(command);
                return Results.Ok(new ApiResponse<int>(id, "Borrower created successfully"));
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateBorrowerCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest(new ApiResponse<string>("ID mismatch"));
                await sender.Send(command);
                return Results.Ok(new ApiResponse<string>("Borrower updated successfully", ""));
            });

            group.MapDelete("/{id}", async (int id, ISender sender) =>
            {
                await sender.Send(new DeleteBorrowerCommand { Id = id });
                return Results.Ok(new ApiResponse<string>("Borrower deleted successfully", ""));
            });
        }
    }
}
