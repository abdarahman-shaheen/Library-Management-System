using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Features.Authors.Commands
{
    public class UpdateAuthorCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
    }

    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand>
    {
        private readonly IGenericRepository<Author> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAuthorCommandHandler(IGenericRepository<Author> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(request.Id);
            if (author == null) return;

            author.Name = request.Name;
            author.Bio = request.Bio;

            await _repository.UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
