using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Features.Authors.Commands
{
    public class CreateAuthorCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
    }

    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, int>
    {
        private readonly IGenericRepository<Author> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAuthorCommandHandler(IGenericRepository<Author> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author
            {
                Name = request.Name,
                Bio = request.Bio
            };

            await _repository.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();

            return author.Id;
        }
    }
}
