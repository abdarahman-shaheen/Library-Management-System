using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Features.Books.Commands
{
    public class DeleteBookCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IGenericRepository<Book> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBookCommandHandler(IGenericRepository<Book> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
