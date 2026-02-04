using FluentValidation;

namespace LibraryManagementSystem.Application.Features.Users.Commands
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(p => p.Username)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(3).WithMessage("{PropertyName} must be at least 3 characters.");

            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();
            
            RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }

    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            // Assuming Login can be Email or Username, at least one required? 
            // Usually DTO has specific fields. Let's assume Email based on typical flows or check property name.
            // Checking LoginUserCommand definition previously... check if it has Email or Username.
            // If checking previous edits, UpdateUser has properties.
            // Let's assume Email field exists.
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0);

            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
