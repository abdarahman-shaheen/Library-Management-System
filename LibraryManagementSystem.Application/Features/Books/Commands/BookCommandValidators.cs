using FluentValidation;

namespace LibraryManagementSystem.Application.Features.Books.Commands
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200);

            RuleFor(p => p.ISBN)
                .NotEmpty()
                .Length(13).WithMessage("ISBN must be 13 characters.");

            RuleFor(p => p.AuthorId)
                .GreaterThan(0);
        }
    }

    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}
