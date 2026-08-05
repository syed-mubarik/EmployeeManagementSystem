using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Designation;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IDesignationService
    {
        Task<PagedResult<DesignationDto>> GetPagedDesignationAsync(DesignationQueryParameters queryParameters);
        Task<DesignationDto> GetByIdAsync(int Id);
        Task<DesignationDto> CreateAsync(CreateDesignationDto dto);

        Task  UpdateAsync(UpdateDesignationDto dto);
        Task DeleteAsync(int id);
    }
}
