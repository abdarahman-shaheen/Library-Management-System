using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Application.Common.Wrappers;
using LibraryManagementSystem.Application.Features.Users.Queries;
using LibraryManagementSystem.Application.Features.Users.Commands;

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
                    return Results.Unauthorized(); 
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
