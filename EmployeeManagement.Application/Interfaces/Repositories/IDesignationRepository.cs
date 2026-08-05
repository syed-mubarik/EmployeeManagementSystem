using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IDesignationRepository : IRepository<Designation>
    {
        public Task<bool> ExistsByNameAsync(string name);
        public Task<bool> ExistsByNameExcludingIdAsync(string name, int designationid);
        Task<bool> HasEmployeesAsync(int designationId);
        Task <PagedResult<Designation>> GetPagedDesignationsAsync(DesignationQueryParameters queryParameters);
      
    }
}
