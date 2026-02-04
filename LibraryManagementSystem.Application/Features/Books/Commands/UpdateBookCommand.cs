using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Features.Books.Commands
{
    public class UpdateBookCommand : IRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand>
    {
        private readonly IGenericRepository<Book> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBookCommandHandler(IGenericRepository<Book> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetByIdAsync(request.Id);
            if (book == null) return;

            book.Title = request.Title;
            book.ISBN = request.ISBN;
            book.PublishedDate = request.PublishedDate;
            book.AuthorId = request.AuthorId;

            await _repository.UpdateAsync(book);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
