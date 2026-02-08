using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Application.Common.Dtos;
using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Features.Authors.Queries;
using LibraryManagementSystem.Application.Features.Authors.Commands;

namespace LibraryManagementSystem.API.Endpoints
{
    public static class AuthorEndpoints
    {
        public static void MapAuthorEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/authors").WithTags("Authors").RequireAuthorization();

            group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var authors = await sender.Send(new GetAuthorsQuery(), cancellationToken);
                return Results.Ok(new ApiResponse<IEnumerable<AuthorDto>>(authors, "Authors retrieved successfully"));
            });

            group.MapGet("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var author = await sender.Send(new GetAuthorByIdQuery { Id = id }, cancellationToken);
                return author != null 
                    ? Results.Ok(new ApiResponse<AuthorDto>(author, "Author retrieved successfully")) 
                    : Results.NotFound(new ApiResponse<AuthorDto>("Author not found"));
            });

            group.MapGet("/{id}/with-books", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var author = await sender.Send(new GetAuthorWithBooksQuery { Id = id }, cancellationToken);
                 return author != null 
                    ? Results.Ok(new ApiResponse<object>(author, "Author retrieved successfully")) 
                    : Results.NotFound(new ApiResponse<object>("Author not found"));
            });

            group.MapPost("/", async ([FromBody] CreateAuthorCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var id = await sender.Send(command, cancellationToken);
                return Results.Ok(new ApiResponse<int>(id, "Author created successfully"));
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateAuthorCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id != command.Id) return Results.BadRequest(new ApiResponse<string>("ID mismatch"));
                await sender.Send(command, cancellationToken);
                return Results.Ok(new ApiResponse<string>("Author updated successfully", ""));
            });

            group.MapDelete("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new DeleteAuthorCommand { Id = id }, cancellationToken);
                return Results.Ok(new ApiResponse<string>("Author deleted successfully", ""));
            });
        }
    }
}
