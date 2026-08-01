using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Department;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IDepartmentService
    {
       // Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<PagedResult<DepartmentDto>> GetDepartmentsAsync(DepartmentQueryParameters queryParameters);
        Task<DepartmentDetailDto?> GetByIdAsync(int id);
        Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);

        Task<bool> UpdateAsync(UpdateDepartmentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}