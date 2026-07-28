using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetEmployeeWithDetailsAsync(int id);

        Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync();
        Task<PagedResult<Employee>> GetEmployeesAsync(EmployeeQueryParameters queryParameters);
    }
}