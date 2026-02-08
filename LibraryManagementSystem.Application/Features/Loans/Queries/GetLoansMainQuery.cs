using MediatR;
using AutoMapper;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Application.Common.Dtos;


namespace LibraryManagementSystem.Application.Features.Loans.Queries
{
    public class GetLoansMainQuery : IRequest<IEnumerable<LoanDto>>
    {
    }

    public class GetLoansMainQueryHandler : IRequestHandler<GetLoansMainQuery, IEnumerable<LoanDto>>
    {
        private readonly IGenericRepository<Loan> _repository;
        private readonly IMapper _mapper;

        public GetLoansMainQueryHandler(IGenericRepository<Loan> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoanDto>> Handle(GetLoansMainQuery request, CancellationToken cancellationToken)
        {
            var loans = await _repository.GetAllAsync();
            return loans.Select(l => _mapper.Map<LoanDto>(l));
        }
    }
}
