using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Employees;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();

        Task<EmployeeDetailDto?> GetByIdAsync(int id);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task UpdateAsync(UpdateEmployeeDto dto);

        Task DeleteAsync(int id);
        Task<PagedResult<EmployeeDto>> GetEmployeesAsync(EmployeeQueryParameters queryParameters);
    }
}