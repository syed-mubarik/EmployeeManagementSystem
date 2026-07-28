using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}