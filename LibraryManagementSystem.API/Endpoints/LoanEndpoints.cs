using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Application.Common.Dtos;
using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Features.Loans.Queries;
using LibraryManagementSystem.Application.Features.Loans.Commands;
using LibraryManagementSystem.API.Filters.EndpointFilters;

namespace LibraryManagementSystem.API.Endpoints
{
    public static class LoanEndpoints
    {
        public static void MapLoanEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/loans")
                .WithTags("Loans")
                .RequireAuthorization()
                .AddEndpointFilter<ResponseWrapperFilter>();

            group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
            {
                 var loans = await sender.Send(new GetLoansMainQuery(), cancellationToken);
                return Results.Ok(loans);
            });

            group.MapGet("/with-details", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var loans = await sender.Send(new GetLoansQuery(), cancellationToken);
                return Results.Ok(loans);
            });

            group.MapPost("/", async ([FromBody] CreateLoanCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var id = await sender.Send(command, cancellationToken);
                return Results.Ok(id);
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateLoanCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                await sender.Send(command, cancellationToken);
                return Results.Ok("Loan updated successfully");
            });
        }
    }
}
