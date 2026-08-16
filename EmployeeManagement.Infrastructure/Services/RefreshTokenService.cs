using EmployeeManagement.Application.Configuration;
using Microsoft.Extensions.Options;
using EmployeeManagement.Application.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagement.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public string GenerateToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public string HashToken(string token)
        {
            using var sha256 = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(token);

            var hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
        public DateTime GetExpirationTime()
        {
            return DateTime.UtcNow.AddDays(
                _jwtSettings.RefreshTokenExpirationDays);
        }
    }
}