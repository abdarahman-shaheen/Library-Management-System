using FluentValidation;
using LibraryManagementSystem.Application.Features.Authors.Queries;
using LibraryManagementSystem.Application.Features.Books.Queries;
using LibraryManagementSystem.Application.Features.Borrowers.Queries;
using LibraryManagementSystem.Application.Features.Users.Queries;

namespace LibraryManagementSystem.Application.Validators.QueryValidators
{
    public class GetAuthorByIdQueryValidator : AbstractValidator<GetAuthorByIdQuery>
    {
        public GetAuthorByIdQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }

    public class GetAuthorWithBooksQueryValidator : AbstractValidator<GetAuthorWithBooksQuery>
    {
        public GetAuthorWithBooksQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }

    public class GetBookByIdQueryValidator : AbstractValidator<GetBookByIdQuery>
    {
        public GetBookByIdQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }

    public class GetBorrowerByIdQueryValidator : AbstractValidator<GetBorrowerByIdQuery>
    {
        public GetBorrowerByIdQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }

    public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
