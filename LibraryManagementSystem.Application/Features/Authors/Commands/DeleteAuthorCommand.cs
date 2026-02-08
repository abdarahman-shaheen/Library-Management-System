using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Features.Authors.Commands
{
    public class DeleteAuthorCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand>
    {
        private readonly IGenericRepository<Author> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAuthorCommandHandler(IGenericRepository<Author> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
