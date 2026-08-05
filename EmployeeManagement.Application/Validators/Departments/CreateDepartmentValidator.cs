using EmployeeManagement.Application.DTOs.Department;
using FluentValidation;

namespace EmployeeManagement.Application.Validators.Department
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Department Name is required.")
                 .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
