using MediatR;
using AutoMapper;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Application.Common.Dtos;

namespace LibraryManagementSystem.Application.Features.Loans.Queries
{
    public class GetLoansQuery : IRequest<IEnumerable<LoanDto>>
    {
    }

    public class GetLoansQueryHandler : IRequestHandler<GetLoansQuery, IEnumerable<LoanDto>>
    {
        private readonly IGenericRepository<Loan> _repository;
        private readonly IMapper _mapper;

        public GetLoansQueryHandler(IGenericRepository<Loan> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoanDto>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
        {
            var loans = await _repository.GetAllWithIncludesAsync(l => l.Book!, l => l.Borrower!);
            return loans.Select(l => _mapper.Map<LoanDto>(l));
        }
    }
}
