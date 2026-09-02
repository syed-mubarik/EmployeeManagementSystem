using EmployeeManagement.Application.DTOs.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<string> AssignPermissionAsync(AssignPermissionDto dto);  // User permission
        Task<string> AssignPermissionToRoleAsync(AssignRolePermissionDto dto);  // Role permission
    }
}
