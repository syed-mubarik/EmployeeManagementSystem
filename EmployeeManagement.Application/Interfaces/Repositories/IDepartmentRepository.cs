using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByNameExcludingIdAsync(string name, int departmentId);
        Task<PagedResult<Department>> GetDepartmentsAsync(DepartmentQueryParameters queryParameters);
    }
}