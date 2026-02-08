using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Features.Books.Commands
{
    public class CreateBookCommand : IRequest<int>
    {
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }
    }

    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, int>
    {
        private readonly IGenericRepository<Book> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookCommandHandler(IGenericRepository<Book> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book
            {
                Title = request.Title,
                ISBN = request.ISBN,
                PublishedDate = request.PublishedDate,
                AuthorId = request.AuthorId
            };

            await _repository.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();

            return book.Id;
        }
    }
}
