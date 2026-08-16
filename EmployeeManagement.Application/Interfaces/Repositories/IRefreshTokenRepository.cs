using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
    }
}