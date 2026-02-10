using System.Security.Claims;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
