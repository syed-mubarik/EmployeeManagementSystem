using EmployeeManagement.Application.DTOs.Employees;
using FluentValidation;

namespace EmployeeManagement.Application.Validators.Employees
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.FirstName)
                 .NotEmpty()
                 .WithMessage("First Name is required.")
                 .MaximumLength(100)
                 .MinimumLength(2);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last Name is required.")
                .MaximumLength(100)
                .MinimumLength(2);

            RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(15)
                .MinimumLength(10);

            RuleFor(x => x.Salary)
                .GreaterThan(0);

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .WithMessage("Please select a department.");
            
            RuleFor(x => x.DesignationId)
                .GreaterThan(0)
                .WithMessage("Please select a designation.");

            RuleFor(x => x.DateOfBirth)
           .NotEqual(DateTime.MinValue)
           .WithMessage("Date of Birth is required.")
           .LessThan(DateTime.Today)
           .WithMessage("Date of Birth cannot be today or in the future.");

            RuleFor(x => x.JoiningDate)
            .NotEqual(DateTime.MinValue)
            .WithMessage("Joining Date is required.")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Joining Date cannot be in the future.");

            RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage("Please select a valid gender.");
        }
    }
}