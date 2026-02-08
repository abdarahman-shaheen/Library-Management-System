using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Features.Loans.Commands
{
    public class UpdateLoanCommand : IRequest
    {
        public int Id { get; set; }
        public DateTime ReturnDate { get; set; }
    }

    public class UpdateLoanCommandHandler : IRequestHandler<UpdateLoanCommand>
    {
        private readonly IGenericRepository<Loan> _repository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLoanCommandHandler(IGenericRepository<Loan> repository, IGenericRepository<Book> bookRepository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (loan == null) return;

            loan.ReturnDate = request.ReturnDate;
            await _repository.UpdateAsync(loan, cancellationToken);

            var book = await _bookRepository.GetByIdAsync(loan.BookId, cancellationToken);
            if (book != null)
            {
                book.IsBorrowed = false;
                await _bookRepository.UpdateAsync(book, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
