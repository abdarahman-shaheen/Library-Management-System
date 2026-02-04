using FluentValidation;

namespace LibraryManagementSystem.Application.Features.Loans.Commands
{
    public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
    {
        public CreateLoanCommandValidator()
        {
            RuleFor(p => p.BookId)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.BorrowerId)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }

    public class UpdateLoanCommandValidator : AbstractValidator<UpdateLoanCommand>
    {
        public UpdateLoanCommandValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            
            // ReturnDate validation could be added here if it was part of the command payload, 
            // but currently UpdateLoanCommand usually just sets it to DateTime.Now in handler or takes it.
            // If the command has ReturnDate property:
            // RuleFor(p => p.ReturnDate).GreaterThan(DateTime.MinValue);
        }
    }
}
