using AutoMapper;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Application.Common.Dtos;

namespace LibraryManagementSystem.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<Book, BookDto>();
            CreateMap<Loan, LoanDto>();
            CreateMap<Borrower, BorrowerDto>();
        }
    }
}
