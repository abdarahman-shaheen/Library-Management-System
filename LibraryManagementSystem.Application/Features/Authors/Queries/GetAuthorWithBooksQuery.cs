using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Application.Common.Dtos;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Features.Authors.Queries
{
    public class GetAuthorWithBooksQuery : IRequest<AuthorDto?>
    {
        public int Id { get; set; }
    }

    public class GetAuthorWithBooksQueryHandler : IRequestHandler<GetAuthorWithBooksQuery, AuthorDto?>
    {
        private readonly IGenericRepository<Author> _repository;

        public GetAuthorWithBooksQueryHandler(IGenericRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<AuthorDto?> Handle(GetAuthorWithBooksQuery request, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdWithIncludesAsync(request.Id, a => a.Books!);
            if (author == null) return null;

            var dto = new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Bio = author.Bio,
                Books = author.Books?.Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    PublishedDate = b.PublishedDate,
                    AuthorId = b.AuthorId
                }).ToList() ?? new List<BookDto>()
            };

            return dto;
        }
    }
}
