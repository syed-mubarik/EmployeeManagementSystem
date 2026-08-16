using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeManagement.Application.Configuration;
using EmployeeManagement.Application.DTOs.Authentication;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public AccessTokenResultDto GenerateAccessToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(
            _jwtSettings.AccessTokenExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AccessTokenResultDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt
        };
    }
}