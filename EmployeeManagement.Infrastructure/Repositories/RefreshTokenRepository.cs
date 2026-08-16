using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
        {
            return await _dbSet
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
        }
    }
}