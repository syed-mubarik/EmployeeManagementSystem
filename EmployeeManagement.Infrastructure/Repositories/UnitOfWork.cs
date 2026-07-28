using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Persistence.Context;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IEmployeeRepository Employees { get; }

        public UnitOfWork(ApplicationDbContext context,IEmployeeRepository employeeRepository)
        {
            _context = context;
            Employees = employeeRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}