using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Departments
                .AnyAsync(d => d.Name == name && !d.IsDeleted);
        }
        public async Task<bool> ExistsByNameExcludingIdAsync(string name, int departmentId)
        {
            return await _context.Departments
                          .AnyAsync(d =>
                          d.Name == name &&
                          d.Id != departmentId &&
                         !d.IsDeleted);
        }
        public async Task<PagedResult<Department>> GetDepartmentsAsync(DepartmentQueryParameters queryParameters)
        {
            var query = GetDepartmentQuery();

            // Search
            if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                // first clean the input
                var SearchTerm = queryParameters.SearchTerm.Trim();

                query = query.Where(d =>
                d.Name.Contains(SearchTerm) ||
                (d.Description != null &&   // description can be null so we added a check
                d.Description.Contains(SearchTerm))
                );
            }
            // Sorting
            switch (queryParameters.SortBy?.ToLower())
            {
                case "name":
                    query = queryParameters.Descending
                          ? query.OrderByDescending(d => d.Name)
                          : query.OrderBy(d => d.Name);
                    break;
                case "createdat":
                    query = queryParameters.Descending
                          ? query.OrderByDescending(d => d.CreatedAt)
                                  .ThenByDescending(e => e.Id)
                          : query.OrderBy(d => d.CreatedAt)
                                  .ThenBy(e => e.Id);
                    break;

                default:
                    query = query.OrderBy(d => d.Id);
                    break;
            }
            // Count
            var TotalCounts = await query.CountAsync();

            //Pagination
            query = query.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                   .Take(queryParameters.PageSize);

            var departments = await query.ToListAsync();

            return new PagedResult<Department>
            {
                Items = departments,
                TotalCount = TotalCounts,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };
        }

        
        // Helper private query
        private IQueryable<Department> GetDepartmentQuery()
        {
            return _context.Departments
                .AsNoTracking()
                .Where(d => !d.IsDeleted);
        }
    }
}