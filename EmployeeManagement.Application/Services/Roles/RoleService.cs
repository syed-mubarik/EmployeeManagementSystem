using EmployeeManagement.Application.DTOs.Roles;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Services.Roles;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<string> CreateRoleAsync(CreateRoleDto dto)
    {
        var roleName = dto.RoleName.Trim();

        // Check whether the role already exists
        var roleExists = await _roleManager.RoleExistsAsync(roleName);

        if (roleExists)
        {
            return "Role already exists.";
        }

        // Create the role
        var result = await _roleManager.CreateAsync(
            new IdentityRole(roleName));

        if (!result.Succeeded)
        {
            return string.Join(
                ", ",
                result.Errors.Select(e => e.Description));
        }

        return "Role created successfully.";
    }

    public async Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        return await _roleManager.Roles
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name!
            })
            .ToListAsync();
    }
    public async Task<string> UpdateRoleAsync(UpdateRoleDto dto)
    {
        var role = await _roleManager.FindByIdAsync(dto.Id);

        if (role == null)
        {
            return "Role not found.";
        }

        var roleName = dto.RoleName.Trim();

        var roleExists = await _roleManager.RoleExistsAsync(roleName);

        if (roleExists && role.Name != roleName)
        {
            return "Role already exists.";
        }

        role.Name = roleName;

        var result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded)
        {
            return string.Join(
                ", ",
                result.Errors.Select(e => e.Description));
        }

        return "Role updated successfully.";
    }
    public async Task<string> DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        if (role == null)
        {
            return "Role not found.";
        }

        var result = await _roleManager.DeleteAsync(role);

        if (!result.Succeeded)
        {
            return string.Join(
                ", ",
                result.Errors.Select(e => e.Description));
        }

        return "Role deleted successfully.";
    }

    public async Task<string> AssignRoleAsync(AssignRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user == null)
        {
            return "User not found.";
        }

        var roleExists = await _roleManager.RoleExistsAsync(dto.RoleName);

        if (!roleExists)
        {
            return "Role not found.";
        }

        var isAlreadyInRole = await _userManager.IsInRoleAsync(
            user,
            dto.RoleName);

        if (isAlreadyInRole)
        {
            return "User already has this role.";
        }

        var result = await _userManager.AddToRoleAsync(
            user,
            dto.RoleName);

        if (!result.Succeeded)
        {
            return string.Join(
                ", ",
                result.Errors.Select(e => e.Description));
        }

        return "Role assigned successfully.";
    }
}