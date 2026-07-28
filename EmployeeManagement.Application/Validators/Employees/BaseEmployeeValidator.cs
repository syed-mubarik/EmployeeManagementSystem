using FluentValidation;

namespace EmployeeManagement.Application.Validators.Employees
{
    // implement it later during a refactoring phase
    public abstract class BaseEmployeeValidator<T> : AbstractValidator<T>
    {
        //protected void ApplyCommonRules(
        //    Func<T, string> firstName,
        //    Func<T, string> lastName,
        //    Func<T, string> email,
        //    Func<T, decimal> salary,
        //    Func<T, int> departmentId,
        //    Func<T, int> designationId)
        //{
        //    RuleFor(firstName)
        //        .NotEmpty()
        //        .MaximumLength(50);

        //    RuleFor(lastName)
        //        .NotEmpty()
        //        .MaximumLength(50);

        //    RuleFor(email)
        //        .NotEmpty()
        //        .EmailAddress();

        //    RuleFor(salary)
        //        .GreaterThan(0);

        //    RuleFor(departmentId)
        //        .GreaterThan(0);

        //    RuleFor(designationId)
        //        .GreaterThan(0);
        //}
    }
}