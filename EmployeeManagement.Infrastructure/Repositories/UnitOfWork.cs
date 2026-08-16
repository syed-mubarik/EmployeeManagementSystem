using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Persistence.Context;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IEmployeeRepository Employees { get; }
        public IDepartmentRepository Departments { get; }
        public IDesignationRepository Designations { get; }
        public IRefreshTokenRepository RefreshTokens { get; }

        public UnitOfWork(ApplicationDbContext context,IEmployeeRepository employeeRepository,
            IDepartmentRepository departments, IDesignationRepository designations, IRefreshTokenRepository refreshTokens)
        {
            _context = context;
            Employees = employeeRepository;
            Departments = departments;
            Designations = designations;
            RefreshTokens = refreshTokens;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}