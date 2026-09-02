using EmployeeManagement.Application.DTOs.Roles;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleDto dto)
    {
        var result = await _roleService.CreateRoleAsync(dto);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleService.GetRolesAsync();

        return Ok(roles);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRole(UpdateRoleDto dto)
    {
        var result = await _roleService.UpdateRoleAsync(dto);

        return Ok(result);
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        var result = await _roleService.DeleteRoleAsync(roleId);

        return Ok(result);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRole(AssignRoleDto dto)
    {
        var result = await _roleService.AssignRoleAsync(dto);

        return Ok(result);
    }
}