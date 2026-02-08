using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Features.Borrowers.Commands
{
    public class DeleteBorrowerCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteBorrowerCommandHandler : IRequestHandler<DeleteBorrowerCommand>
    {
        private readonly IGenericRepository<Borrower> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBorrowerCommandHandler(IGenericRepository<Borrower> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteBorrowerCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
