using EmployeeManagement.Application.DTOs.Department;
using FluentValidation;

namespace EmployeeManagement.Application.Validators.Department
{
    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid Department Id.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("Description cannot exceed 500 characters.");
        }
    }
}