using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Application.Common.Dtos;
using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Features.Books.Queries;
using LibraryManagementSystem.Application.Features.Books.Commands;

namespace LibraryManagementSystem.API.Endpoints
{
    public static class BookEndpoints
    {
        public static void MapBookEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/books").WithTags("Books").RequireAuthorization();

            group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var books = await sender.Send(new GetBooksQuery(), cancellationToken);
                return Results.Ok(new ApiResponse<IEnumerable<BookDto>>(books, "Books retrieved successfully"));
            });

            group.MapGet("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var book = await sender.Send(new GetBookByIdQuery { Id = id }, cancellationToken);
                return book != null 
                    ? Results.Ok(new ApiResponse<BookDto>(book, "Book retrieved successfully")) 
                    : Results.NotFound(new ApiResponse<BookDto>("Book not found"));
            });

            group.MapPost("/", async ([FromBody] CreateBookCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var id = await sender.Send(command, cancellationToken);
                return Results.Ok(new ApiResponse<int>(id, "Book created successfully"));
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateBookCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id != command.Id) return Results.BadRequest(new ApiResponse<string>("ID mismatch"));
                await sender.Send(command, cancellationToken);
                return Results.Ok(new ApiResponse<string>("Book updated successfully", ""));
            });

            group.MapDelete("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new DeleteBookCommand { Id = id }, cancellationToken);
                return Results.Ok(new ApiResponse<string>("Book deleted successfully", ""));
            });
        }
    }
}
