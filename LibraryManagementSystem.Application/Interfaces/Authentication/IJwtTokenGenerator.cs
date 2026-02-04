using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
