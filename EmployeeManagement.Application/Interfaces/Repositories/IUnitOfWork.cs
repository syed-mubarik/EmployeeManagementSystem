namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }

        IDepartmentRepository Departments { get; }
        IDesignationRepository Designations { get; }
        Task<int> SaveChangesAsync();
    }
}