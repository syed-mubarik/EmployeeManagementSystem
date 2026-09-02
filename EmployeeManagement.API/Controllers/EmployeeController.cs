using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Employees;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAuthorizationService _authorizationService;
        public EmployeesController(IEmployeeService employeeService, IAuthorizationService authorizationService)
        {
            _employeeService = employeeService;
            _authorizationService = authorizationService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var employees = await _employeeService.GetAllAsync();

        //    return Ok(employees);
        //}

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Get the requested Employee
            var employee = await _employeeService.GetEmployeeForAuthorizationAsync(id);

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            // Authorize the current user against the Employee resource
            var authorizationResult = await _authorizationService
              //  .AuthorizeAsync(User, employee, "EmployeeOwner");
                .AuthorizeAsync(User, employee, "EmployeeAccess");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            // Employee is already loaded, so don't query the database again.
            var employeeDto = _employeeService.MapToDetailDto(employee);

            return Ok(employeeDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            var employee = await _employeeService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = employee.Id },
                employee);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,UpdateEmployeeDto dto)
        {

            if(id != dto.Id)
            {
                return BadRequest("Route Id and DTo Id do not match.");
            }

            var employee = await _employeeService.GetEmployeeForAuthorizationAsync(id);

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            var authorizationResult =   await _authorizationService.AuthorizeAsync(User,employee,"EmployeeAccess");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            // Only Admin and HR Manager can perform full update
            if (!User.IsInRole("Admin") &&  !User.IsInRole("HR Manager"))
            {
                return Forbid();
            }

            // Update logic comes here
            await _employeeService.UpdateAsync(dto);
            return NoContent();
        }

        // Employee — self update
        [HttpPut("{id:int}/self")]
        public async Task<IActionResult> UpdateSelf(int id,EmployeeSelfUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Route Id and DTO Id do not match.");
            }

            var employee = await _employeeService.GetEmployeeForAuthorizationAsync(id);

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

   var authorizationResult =await _authorizationService.AuthorizeAsync(User, employee, "EmployeeOwner");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _employeeService.UpdateSelfAsync(dto);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "CanDeleteEmployee")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResult<EmployeeDto>>> GetEmployees(
              [FromQuery] EmployeeQueryParameters queryParameters)
        {
            var result = await _employeeService.GetEmployeesAsync(queryParameters);

            return Ok(result);
        }
    }
}