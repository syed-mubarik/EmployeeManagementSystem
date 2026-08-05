using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class DesignationRepository : Repository<Designation>, IDesignationRepository
    {
        public DesignationRepository(ApplicationDbContext context) : base(context) 
        {

        }
        public async Task <bool> ExistsByNameAsync(string name)
        {
            return await _context.Designations
                .AnyAsync(d =>  d.Name == name && !d.IsDeleted);
        }

        public async Task<bool> ExistsByNameExcludingIdAsync(string name, int designationid)
        {
            return await _context.Designations
                        .AnyAsync(d =>
                         d.Name == name &&
                         d.Id != designationid &&
                         !d.IsDeleted);
        }

        public async Task<bool> HasEmployeesAsync(int designationId)
        {
            return await _context.Employees
                .AnyAsync(e =>
                e.DesignationId == designationId &&
                !e.IsDeleted);
        }
        public async Task<PagedResult<Designation>> GetPagedDesignationsAsync(DesignationQueryParameters queryParameters)
        {
            IQueryable<Designation> query = _dbSet     
                                            .Where(d => !d.IsDeleted);

            // Searching
            if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                // first clean the input
                var searchTerm = queryParameters.SearchTerm.Trim();

                query = query.Where(d =>
                               d.Name.Contains(searchTerm) ||
                               (d.Description != null &&
                                d.Description.Contains(searchTerm))
                                );
            }
                // Sorting
                switch(queryParameters.SortBy?.ToLower())
                {
                    case "name":
                     query = queryParameters.Descending 
                        ? query.OrderByDescending(d => d.Name)
                        : query.OrderBy(d => d.Name);
                        break;

                    case "createdat":
                    query = queryParameters.Descending
                        ? query.OrderByDescending(d => d.CreatedAt)
                        : query.OrderBy(d => d.CreatedAt);
                        break;

                        default:
                        query = query.OrderBy(d => d.Name);
                        break;
            }

                // Count 
                var totalCount = await query.CountAsync();

                // Pagination
                query = query.Skip((queryParameters.PageNumber -1) * queryParameters.PageSize)
                             .Take(queryParameters.PageSize);
                
                var designations = await query.ToListAsync();

                return new PagedResult<Designation> 
                { 
                    Items = designations,
                    TotalCount = totalCount,
                    PageNumber = queryParameters.PageNumber,
                    PageSize = queryParameters.PageSize
                };
            
        }

    }
}
