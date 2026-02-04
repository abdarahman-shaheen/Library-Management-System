using AutoMapper;
using LibraryManagementSystem.Application.Common.Dtos;
using LibraryManagementSystem.Domain.Entities;

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
