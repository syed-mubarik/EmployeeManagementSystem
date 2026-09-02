using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync()
        {
            return await GetEmployeeQuery().ToListAsync();
        }

        public async Task<Employee?> GetEmployeeWithDetailsAsync(int id)
        {
            return await GetEmployeeQuery()
        .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<PagedResult<Employee>> GetEmployeesAsync(EmployeeQueryParameters queryParameters)
        {
            var query = GetEmployeeQuery();
            // Search
            if (!String.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                // first clean the input
                var searchTerm = queryParameters.SearchTerm.Trim();
                query = query.Where(e =>
                e.FirstName.Contains(searchTerm) ||
                e.LastName.Contains(searchTerm) ||
                e.Email.Contains(searchTerm) ||
                e.EmployeeCode.Contains(searchTerm)
                );
            }

            //   Filter
            if (queryParameters.DepartmentId.HasValue)
                {
                    query = query.Where(e =>
                    e.DepartmentId == queryParameters.DepartmentId.Value);
                }
                if (queryParameters.DesignationId.HasValue)
                {
                    query = query.Where(e =>
                    e.DesignationId == queryParameters.DesignationId.Value
                    );
                }
            // Dynamic Sorting

            if (queryParameters.SortBy?.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase) == true)
            {
                //if (queryParameters.Descending)
                //{
                //    query = query.OrderByDescending(e => e.CreatedAt);
                //}

                // if the frontend sends nothing e.g; SortBy = null
                query = queryParameters.Descending
               ? query.OrderByDescending(e => e.CreatedAt)
                      .ThenByDescending(e => e.Id)
               : query.OrderBy(e => e.CreatedAt)
                      .ThenBy(e => e.Id);  // With ThenBy, the order is always predictable.
            }
            else
            {
                query = query.OrderByDescending(e => e.CreatedAt)
                             .ThenByDescending(e => e.Id);
            }
            

            // CountAsync
            var totalRecords = await query.CountAsync();

            // apply pagination:
            query =  query.Skip((queryParameters.PageNumber -  1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize);
            var employees = await query.ToListAsync();
            return new PagedResult<Employee>
            {
                Items = employees,
                TotalCount = totalRecords,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };

        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Employees
                .AnyAsync(e => e.Id == id && !e.IsDeleted);
        }
        // Helper private query
        private IQueryable<Employee> GetEmployeeQuery()
        {
            return _context.Employees
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .Include(e => e.Department)
                .Include(e => e.Designation);
        }
    }
}