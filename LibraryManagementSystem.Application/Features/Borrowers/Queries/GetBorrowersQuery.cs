using MediatR;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Application.Common.Dtos;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Features.Borrowers.Queries
{
    public class GetBorrowersQuery : IRequest<IEnumerable<BorrowerDto>>
    {
    }

    public class GetBorrowersQueryHandler : IRequestHandler<GetBorrowersQuery, IEnumerable<BorrowerDto>>
    {
        private readonly IGenericRepository<Borrower> _repository;
        private readonly IMapper _mapper;

        public GetBorrowersQueryHandler(IGenericRepository<Borrower> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BorrowerDto>> Handle(GetBorrowersQuery request, CancellationToken cancellationToken)
        {
            var borrowers = await _repository.GetAllAsync();
            return borrowers.Select(b => _mapper.Map<BorrowerDto>(b));
        }
    }
}
