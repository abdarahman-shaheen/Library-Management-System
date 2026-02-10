using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Application.Interfaces.Authentication;
using LibraryManagementSystem.Application.Common.Dtos;

namespace LibraryManagementSystem.Application.Features.Users.Commands
{
    public class RefreshTokenCommand : IRequest<AuthResponseDto>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(IGenericRepository<User> repository, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                throw new Exception("Invalid token");
            }

            var user = await _repository.GetByIdAsync(userId, cancellationToken);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new Exception("Invalid refresh token");
            }

            var newAccessToken = _jwtTokenGenerator.GenerateToken(user);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _repository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
