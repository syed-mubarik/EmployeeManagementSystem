using EmployeeManagement.Application.DTOs.Authentication;
using FluentValidation;

namespace EmployeeManagement.Application.Validators.Authentication
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.Password)
                .WithMessage("Password and Confirm Password do not match.");
        }
    }
}