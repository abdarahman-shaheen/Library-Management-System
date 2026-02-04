using FluentValidation;

namespace LibraryManagementSystem.Application.Features.Books.Commands
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.ISBN)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Length(13).WithMessage("ISBN must be exactly 13 characters.");

            RuleFor(p => p.AuthorId)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}
