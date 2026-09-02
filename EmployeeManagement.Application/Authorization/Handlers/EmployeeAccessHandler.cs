using EmployeeManagement.Application.Authorization.Requirements;
using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Authorization.Handlers
{
    public class EmployeeAccessHandler: AuthorizationHandler<EmployeeAccessRequirement, Employee>
    {
     protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,EmployeeAccessRequirement requirement,Employee employee)
        {
            // Admin can access any employee
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // HR Manager can access any employee
            if (context.User.IsInRole("HR Manager"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Other users must own the employee
            var employeeIdClaim =context.User.FindFirst("EmployeeId");

            if (employeeIdClaim == null)
                return Task.CompletedTask;

            if (!int.TryParse(employeeIdClaim.Value,out var userEmployeeId))
            {
                return Task.CompletedTask;
            }

            if (userEmployeeId == employee.Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
