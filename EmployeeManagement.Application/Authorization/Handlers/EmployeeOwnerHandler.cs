using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

public class EmployeeOwnerHandler
    : AuthorizationHandler<EmployeeOwnerRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        EmployeeOwnerRequirement requirement)
    {
        // 1. Get EmployeeId from the authenticated user's claims
        var employeeIdClaim = context.User.FindFirst("EmployeeId");

        if (employeeIdClaim == null)
            return Task.CompletedTask;

        // 2. Get the requested employee from the resource
        if (context.Resource is not Employee employee)
            return Task.CompletedTask;

        // 3. Convert the claim value to an integer
        if (!int.TryParse(employeeIdClaim.Value, out var userEmployeeId))
            return Task.CompletedTask;

        // 4. Compare logged-in user's EmployeeId with requested Employee.Id
        if (userEmployeeId == employee.Id)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}