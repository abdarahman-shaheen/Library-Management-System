using MediatR;
using System.Text;
using System.Security.Cryptography;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Application.Interfaces.Authentication;
using LibraryManagementSystem.Application.Common.Dtos;

namespace LibraryManagementSystem.Application.Features.Users.Commands
{
    public class LoginUserCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponseDto>
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public LoginUserCommandHandler(IGenericRepository<User> repository, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var users = await _repository.FindAsync(u => u.Email == request.Email, cancellationToken);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(request.Password);
            var hashBytes = sha256.ComputeHash(passwordBytes);
            var hash = Convert.ToBase64String(hashBytes);

            if (user.PasswordHash != hash)
            {
                throw new Exception("Invalid credentials");
            }

            var accessToken = _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set expiry time (e.g., 7 days)

            await _repository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
