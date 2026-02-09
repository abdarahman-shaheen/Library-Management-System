using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Application.Common.Dtos;
using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Features.Books.Queries;
using LibraryManagementSystem.Application.Features.Books.Commands;
using LibraryManagementSystem.API.Filters.EndpointFilters;

namespace LibraryManagementSystem.API.Endpoints
{
    public static class BookEndpoints
    {
        public static void MapBookEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/books")
                .WithTags("Books")
                .RequireAuthorization()
                .AddEndpointFilter<ResponseWrapperFilter>();

            group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var books = await sender.Send(new GetBooksQuery(), cancellationToken);
                return books;
            });

            group.MapGet("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var book = await sender.Send(new GetBookByIdQuery { Id = id }, cancellationToken);
                return book is not null ? Results.Ok(book) : Results.NotFound();
            });

            group.MapPost("/", async ([FromBody] CreateBookCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var id = await sender.Send(command, cancellationToken);
                return Results.Ok(id);
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateBookCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                await sender.Send(command, cancellationToken);
                return Results.Ok("Book updated successfully");
            });

            group.MapDelete("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new DeleteBookCommand { Id = id }, cancellationToken);
                return Results.Ok("Book deleted successfully");
            });
        }
    }
}
