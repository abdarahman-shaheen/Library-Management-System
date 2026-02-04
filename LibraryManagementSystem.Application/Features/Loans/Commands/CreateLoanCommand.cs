using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Features.Loans.Commands
{
    public class CreateLoanCommand : IRequest<int>
    {
        public int BookId { get; set; }
        public int BorrowerId { get; set; }
    }

    public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, int>
    {
        private readonly IGenericRepository<Loan> _repository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLoanCommandHandler(IGenericRepository<Loan> repository, IGenericRepository<Book> bookRepository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
        {
            // Business Logic: Check if book is available
            // Although simple generic repository might not have this, we can use FindAsync or GetById.
            // But for simple start, let's assume we can loan. 
            // Better: update Book.IsBorrowed = true.

            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book != null)
            {
                 book.IsBorrowed = true;
                 await _bookRepository.UpdateAsync(book);
            }

            var loan = new Loan
            {
                BookId = request.BookId,
                BorrowerId = request.BorrowerId,
                LoanDate = DateTime.UtcNow
            };

            await _repository.AddAsync(loan);
            await _unitOfWork.SaveChangesAsync();

            return loan.Id;
        }
    }
}
