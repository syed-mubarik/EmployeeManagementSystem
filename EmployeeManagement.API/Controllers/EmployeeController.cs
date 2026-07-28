using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Employees;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var employees = await _employeeService.GetAllAsync();

        //    return Ok(employees);
        //}
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            return Ok(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            var employee = await _employeeService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = employee.Id },
                employee);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateEmployeeDto dto)
        {
            await _employeeService.UpdateAsync(dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<PagedResult<EmployeeDto>>> GetEmployees(
              [FromQuery] EmployeeQueryParameters queryParameters)
        {
            var result = await _employeeService.GetEmployeesAsync(queryParameters);

            return Ok(result);
        }
    }
}