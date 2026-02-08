using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Features.Borrowers.Commands
{
    public class UpdateBorrowerCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class UpdateBorrowerCommandHandler : IRequestHandler<UpdateBorrowerCommand>
    {
        private readonly IGenericRepository<Borrower> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBorrowerCommandHandler(IGenericRepository<Borrower> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateBorrowerCommand request, CancellationToken cancellationToken)
        {
            var borrower = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (borrower == null) return;

            borrower.Name = request.Name;
            borrower.Email = request.Email;
            borrower.Phone = request.Phone;

            await _repository.UpdateAsync(borrower, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
