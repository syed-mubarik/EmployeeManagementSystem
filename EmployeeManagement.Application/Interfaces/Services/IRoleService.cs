using EmployeeManagement.Application.DTOs.Roles;

namespace EmployeeManagement.Application.Interfaces.Services;

public interface IRoleService
{
    Task<string> CreateRoleAsync(CreateRoleDto dto);
    Task<IEnumerable<RoleDto>> GetRolesAsync();
    Task<string> UpdateRoleAsync(UpdateRoleDto dto);
    Task<string> DeleteRoleAsync(string roleId);
    Task<string> AssignRoleAsync(AssignRoleDto dto);
}