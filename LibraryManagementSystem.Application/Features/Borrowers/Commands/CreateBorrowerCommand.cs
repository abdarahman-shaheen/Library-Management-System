using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Features.Borrowers.Commands
{
    public class CreateBorrowerCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class CreateBorrowerCommandHandler : IRequestHandler<CreateBorrowerCommand, int>
    {
        private readonly IGenericRepository<Borrower> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBorrowerCommandHandler(IGenericRepository<Borrower> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateBorrowerCommand request, CancellationToken cancellationToken)
        {
            var borrower = new Borrower
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone
            };

            await _repository.AddAsync(borrower);
            await _unitOfWork.SaveChangesAsync();

            return borrower.Id;
        }
    }
}
