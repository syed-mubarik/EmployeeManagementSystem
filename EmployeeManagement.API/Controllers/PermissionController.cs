using EmployeeManagement.Application.DTOs.Authorization;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignPermission(AssignPermissionDto dto)
    {
        var result = await _permissionService.AssignPermissionAsync(dto);

        return Ok(result);
    }

    [HttpPost("assign-to-role")]
    public async Task<IActionResult> AssignPermissionToRole(AssignRolePermissionDto dto)
    {
        var result =
            await _permissionService.AssignPermissionToRoleAsync(dto);

        return Ok(result);
    }
}