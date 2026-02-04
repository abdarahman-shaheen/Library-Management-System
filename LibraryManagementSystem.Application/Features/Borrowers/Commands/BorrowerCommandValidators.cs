using FluentValidation;

namespace LibraryManagementSystem.Application.Features.Borrowers.Commands
{
    public class CreateBorrowerCommandValidator : AbstractValidator<CreateBorrowerCommand>
    {
        public CreateBorrowerCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100);

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("A valid email is required.");
            
            RuleFor(p => p.Phone)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }

    public class UpdateBorrowerCommandValidator : AbstractValidator<UpdateBorrowerCommand>
    {
        public UpdateBorrowerCommandValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100);

            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }

    public class DeleteBorrowerCommandValidator : AbstractValidator<DeleteBorrowerCommand>
    {
        public DeleteBorrowerCommandValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}
