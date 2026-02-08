using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // Password update logic usually separate, but keeping simple here or omitting for brevity if not strictly asked.
        // Requirement: "PUT /api/Users/{id}: Update profile"
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IGenericRepository<User> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null) return;

            user.Username = request.Username;
            user.Email = request.Email;

            await _repository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
