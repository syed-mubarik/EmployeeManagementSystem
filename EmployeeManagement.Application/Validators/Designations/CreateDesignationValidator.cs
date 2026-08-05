using EmployeeManagement.Application.DTOs.Designation;
using FluentValidation;

namespace EmployeeManagement.Application.Validators.Designations
{
    public class CreateDesignationValidator : AbstractValidator<CreateDesignationDto>
    {
       public CreateDesignationValidator() 
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
