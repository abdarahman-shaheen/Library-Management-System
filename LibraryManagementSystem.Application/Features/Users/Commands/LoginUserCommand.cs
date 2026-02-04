using MediatR;
using LibraryManagementSystem.Application.Interfaces.Authentication;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Features.Users.Commands
{
    public class LoginUserCommand : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(IGenericRepository<User> repository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _repository = repository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var users = await _repository.FindAsync(u => u.Email == request.Email);
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

            return _jwtTokenGenerator.GenerateToken(user);
        }
    }
}
