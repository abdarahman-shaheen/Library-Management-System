using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Features.Users.Queries;
using LibraryManagementSystem.Application.Features.Users.Commands;
using LibraryManagementSystem.API.Infrastructure;

namespace LibraryManagementSystem.API.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Users")
                .WithTags("Users")
                .RequireAuthorization()
                .AddEndpointFilter<ResponseWrapperFilter>();

            group.MapGet("/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var user = await sender.Send(new GetUserByIdQuery { Id = id }, cancellationToken);
                return user is not null ? Results.Ok(user) : Results.NotFound();
            });

            group.MapPost("/", async ([FromBody] RegisterUserCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var id = await sender.Send(command, cancellationToken);
                return Results.Ok(id);
            }).AllowAnonymous();
            
            group.MapPost("/login", async ([FromBody] LoginUserCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                try 
                {
                    var token = await sender.Send(command, cancellationToken);
                    return Results.Ok(new { Token = token });
                }
                catch
                {
                    return Results.Unauthorized(); 
                }
            }).AllowAnonymous();

            group.MapPut("/{id}", async (int id, [FromBody] UpdateUserCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                await sender.Send(command, cancellationToken);
                return Results.Ok("User updated successfully");
            });
        }
    }
}
