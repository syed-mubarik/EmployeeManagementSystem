using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Domain.Common;
using EmployeeManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}