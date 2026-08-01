using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Persistence.Context;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IEmployeeRepository Employees { get; }
        public IDepartmentRepository Departments { get; }

        public UnitOfWork(ApplicationDbContext context,IEmployeeRepository employeeRepository, IDepartmentRepository departments)
        {
            _context = context;
            Employees = employeeRepository;
            Departments = departments;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}