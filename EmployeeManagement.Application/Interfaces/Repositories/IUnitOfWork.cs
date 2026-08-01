namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }

        IDepartmentRepository Departments { get; }
        Task<int> SaveChangesAsync();
    }
}