using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var departments = await _departmentService.GetAllAsync();
        //    return Ok(departments);
        //}
        public async Task<ActionResult<PagedResult<DepartmentDto>>> GetDepartments(
            [FromQuery] DepartmentQueryParameters queryParameters)
        {
            var result = await _departmentService.GetDepartmentsAsync(queryParameters);

            return Ok(result);
        }
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var departments = await _departmentService.GetByIdAsync(id);
            return Ok(departments);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            var department = await _departmentService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = department.Id }, department);
        }
        [HttpPut("{id}")]
        public async Task <IActionResult> Update(int id, UpdateDepartmentDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Route Id and DTO Id do not match.");
            }
            await _departmentService.UpdateAsync(dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteAsync(id);

            return NoContent();
        }
    }
}
