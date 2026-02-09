using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Application.Common.Dtos;
using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Features.Borrowers.Queries;
using LibraryManagementSystem.Application.Features.Borrowers.Commands;
using LibraryManagementSystem.API.Filters.EndpointFilters;

namespace LibraryManagementSystem.API.Endpoints
{
    public static class BorrowerEndpoints
    {
        public static void MapBorrowerEndpoints(this IEndpointRouteBuilder app)
        {
             var group = app.MapGroup("/api/borrowers")
                 .WithTags("Borrowers")
                 .RequireAuthorization()
                 .AddEndpointFilter<ResponseWrapperFilter>();

            group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var borrowers = await sender.Send(new GetBorrowersQuery(), cancellationToken);
                return borrowers;
            });

            group.MapGet("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var borrower = await sender.Send(new GetBorrowerByIdQuery { Id = id }, cancellationToken);
                return borrower is not null ? Results.Ok(borrower) : Results.NotFound();
            });

            group.MapPost("/", async ([FromBody] CreateBorrowerCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var id = await sender.Send(command, cancellationToken);
                return Results.Ok(id);
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateBorrowerCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                await sender.Send(command, cancellationToken);
                return Results.Ok("Borrower updated successfully");
            });

            group.MapDelete("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new DeleteBorrowerCommand { Id = id }, cancellationToken);
                return Results.Ok("Borrower deleted successfully");
            });
        }
    }
}
