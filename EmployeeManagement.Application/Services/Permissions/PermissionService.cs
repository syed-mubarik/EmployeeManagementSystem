using System.Security.Claims;
using EmployeeManagement.Application.Authorization.Permissions;
using EmployeeManagement.Application.DTOs.Authorization;
using EmployeeManagement.Application.Exceptions;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class PermissionService : IPermissionService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public PermissionService(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<string> AssignPermissionAsync(AssignPermissionDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user == null)
        {
            return "User not found.";
        }

        var validPermissions = new[]
        {
            Permissions.EmployeeRead,
            Permissions.EmployeeCreate,
            Permissions.EmployeeUpdate,
            Permissions.EmployeeDelete
        };

        if (!validPermissions.Contains(dto.Permission))
        {
            return "Invalid permission.";
        }

        var existingClaims = await _userManager.GetClaimsAsync(user);

        var permissionExists = existingClaims.Any(c =>
            c.Type == "Permission" &&
            c.Value == dto.Permission);

        if (permissionExists)
        {
            return "User already has this permission.";
        }

        var result = await _userManager.AddClaimAsync(
            user,
            new Claim("Permission", dto.Permission));

        if (!result.Succeeded)
        {
            return string.Join(
                ", ",
                result.Errors.Select(e => e.Description));
        }

        return "Permission assigned successfully.";
    }

    public async Task<string> AssignPermissionToRoleAsync(AssignRolePermissionDto dto)
    {
        var role = await _roleManager.FindByIdAsync(dto.RoleId);

        if (role == null)
        {
            throw new NotFoundException("Role not found.");
        }

        var permission = dto.Permission.Trim();

        var existingClaims = await _roleManager.GetClaimsAsync(role);

        var alreadyExists = existingClaims.Any(c =>
            c.Type == "Permission" &&
            c.Value == permission);

        if (alreadyExists)
        {
            return "Permission already assigned to role.";
        }

        var result = await _roleManager.AddClaimAsync(
            role,
            new Claim("Permission", permission));

        if (!result.Succeeded)
        {
            return string.Join(
                ", ",
                result.Errors.Select(e => e.Description));
        }

        return "Permission assigned to role successfully.";
    }
}