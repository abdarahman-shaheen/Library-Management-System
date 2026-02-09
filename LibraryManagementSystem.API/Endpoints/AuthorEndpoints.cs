using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Application.Common.Dtos;
using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Features.Authors.Queries;
using LibraryManagementSystem.Application.Features.Authors.Commands;
using LibraryManagementSystem.API.Filters.EndpointFilters;

namespace LibraryManagementSystem.API.Endpoints
{
    public static class AuthorEndpoints
    {
        public static void MapAuthorEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/authors")
                .WithTags("Authors")
                .RequireAuthorization()
                .AddEndpointFilter<ResponseWrapperFilter>();

            group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var authors = await sender.Send(new GetAuthorsQuery(), cancellationToken);
                return authors;
            });

            group.MapGet("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var author = await sender.Send(new GetAuthorByIdQuery { Id = id }, cancellationToken);
                return author is not null ? Results.Ok(author) : Results.NotFound();
            });

            group.MapGet("/{id}/with-books", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var author = await sender.Send(new GetAuthorWithBooksQuery { Id = id }, cancellationToken);
                 return author is not null ? Results.Ok(author) : Results.NotFound();
            });

            group.MapPost("/", async ([FromBody] CreateAuthorCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var id = await sender.Send(command, cancellationToken);
                return Results.Ok(id);
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateAuthorCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                await sender.Send(command, cancellationToken);
                return Results.Ok("Author updated successfully");
            });

            group.MapDelete("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new DeleteAuthorCommand { Id = id }, cancellationToken);
                return Results.Ok("Author deleted successfully");
            });
        }
    }
}
