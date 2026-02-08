using MediatR;
using AutoMapper;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Application.Common.Dtos;

namespace LibraryManagementSystem.Application.Features.Borrowers.Queries
{
    public class GetBorrowerByIdQuery : IRequest<BorrowerDto?>
    {
        public int Id { get; set; }
    }

    public class GetBorrowerByIdQueryHandler : IRequestHandler<GetBorrowerByIdQuery, BorrowerDto?>
    {
        private readonly IGenericRepository<Borrower> _repository;
        private readonly IMapper _mapper;

        public GetBorrowerByIdQueryHandler(IGenericRepository<Borrower> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BorrowerDto?> Handle(GetBorrowerByIdQuery request, CancellationToken cancellationToken)
        {
            var borrower = await _repository.GetByIdAsync(request.Id);
            return borrower == null ? null : _mapper.Map<BorrowerDto>(borrower);
        }
    }
}
