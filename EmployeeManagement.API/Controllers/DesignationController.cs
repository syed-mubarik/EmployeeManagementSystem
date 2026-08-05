using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Designation;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationsController : ControllerBase
    {
        private readonly IDesignationService _designationService;
        public DesignationsController(IDesignationService designationService)
        {
            _designationService = designationService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<DesignationDto>>> GetDesignations(
            [FromQuery] DesignationQueryParameters queryParameters)
        {
            var result = await _designationService.GetPagedDesignationAsync(queryParameters);

            return Ok(result);

        }
        [HttpGet("{id:int}")]
        public async Task <ActionResult<DesignationDto>> GetById(int id)
        {
            var designation = await _designationService.GetByIdAsync(id);
            return Ok(designation);
        }

        [HttpPost]
        public async Task <ActionResult<DesignationDto>> Create(CreateDesignationDto dto)
        {
            var designations =  await _designationService.CreateAsync(dto);
            return CreatedAtAction(
                    nameof(GetById),
                    new {id = designations.Id}, designations);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateDesignationDto dto)
        {
            if(id != dto.Id)
            {
                return BadRequest("Route Id and DTO Id donot match.");
            } 
            await _designationService.UpdateAsync(dto);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task <IActionResult> Delete(int id)
        {
            await _designationService.DeleteAsync(id);
             return NoContent();
        }
    }
}
