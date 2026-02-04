using MediatR;
using LibraryManagementSystem.Application.Features.Users.Commands;
using LibraryManagementSystem.Application.Features.Users.Queries;
using Microsoft.AspNetCore.Mvc;

using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Common.Dtos; // Note: UserEndpoints might use UserDto if defined, though previous code used object. Adding for consistency or potential future use.

namespace LibraryManagementSystem.API.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Users").WithTags("Users").RequireAuthorization(); // Default require auth

            group.MapGet("/{id}", async (int id, ISender sender) =>
            {
                var user = await sender.Send(new GetUserByIdQuery { Id = id });
                return user != null 
                    ? Results.Ok(new ApiResponse<object>(user, "User retrieved successfully")) // Keeping object since UserDto might not be inferred here easily
                    : Results.NotFound(new ApiResponse<object>("User not found"));
            });

            group.MapPost("/", async ([FromBody] RegisterUserCommand command, ISender sender) =>
            {
                var id = await sender.Send(command);
                // Return 201 with location
                return Results.Ok(new ApiResponse<int>(id, "User registered successfully"));
            }).AllowAnonymous();
            
            group.MapPost("/login", async ([FromBody] LoginUserCommand command, ISender sender) =>
            {
                try 
                {
                    var token = await sender.Send(command);
                    return Results.Ok(new ApiResponse<object>(new { Token = token }, "Login successful"));
                }
                catch
                {
                    return Results.Unauthorized(); // Exception middleware might catch this if it bubbles up, but Unauthorized() is standard 401
                }
            }).AllowAnonymous();

            group.MapPut("/{id}", async (int id, [FromBody] UpdateUserCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest(new ApiResponse<string>("ID mismatch"));
                await sender.Send(command);
                return Results.Ok(new ApiResponse<string>("User updated successfully", ""));
            });
        }
    }
}
