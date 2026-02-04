using MediatR;
using LibraryManagementSystem.Application.Features.Books.Commands;
using LibraryManagementSystem.Application.Features.Books.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Common.Dtos;

namespace LibraryManagementSystem.API.Endpoints
{
    public static class BookEndpoints
    {
        public static void MapBookEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/books").WithTags("Books").RequireAuthorization();

            group.MapGet("/", async (ISender sender) =>
            {
                var books = await sender.Send(new GetBooksQuery());
                return Results.Ok(new ApiResponse<IEnumerable<BookDto>>(books, "Books retrieved successfully"));
            });

            group.MapGet("/{id}", async (int id, ISender sender) =>
            {
                var book = await sender.Send(new GetBookByIdQuery { Id = id });
                return book != null 
                    ? Results.Ok(new ApiResponse<BookDto>(book, "Book retrieved successfully")) 
                    : Results.NotFound(new ApiResponse<BookDto>("Book not found"));
            });

            group.MapPost("/", async ([FromBody] CreateBookCommand command, ISender sender) =>
            {
                var id = await sender.Send(command);
                return Results.Ok(new ApiResponse<int>(id, "Book created successfully"));
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateBookCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest(new ApiResponse<string>("ID mismatch"));
                await sender.Send(command);
                return Results.Ok(new ApiResponse<string>("Book updated successfully", ""));
            });

            group.MapDelete("/{id}", async (int id, ISender sender) =>
            {
                await sender.Send(new DeleteBookCommand { Id = id });
                return Results.Ok(new ApiResponse<string>("Book deleted successfully", ""));
            });
        }
    }
}
