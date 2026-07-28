namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }

        Task<int> SaveChangesAsync();
    }
}