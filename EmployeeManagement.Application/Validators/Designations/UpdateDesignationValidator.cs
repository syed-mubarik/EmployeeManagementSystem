using EmployeeManagement.Application.DTOs.Designation;
using FluentValidation;

namespace EmployeeManagement.Application.Validators.Designations
{
    public class UpdateDesignationValidator : AbstractValidator<UpdateDesignationDto>
    {
        public UpdateDesignationValidator() 
        {
            RuleFor(x => x.Id)
               .GreaterThan(0)
               .WithMessage("Invalid Department Id.");

            RuleFor(x => x.Name)
                        .NotEmpty().WithMessage("Department Name is required.")
                        .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.")
                        .MinimumLength(2);

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description cannot exceed 500 characters.");

        }
    }
}
