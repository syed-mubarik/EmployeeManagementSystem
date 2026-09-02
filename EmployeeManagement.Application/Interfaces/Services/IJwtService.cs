using EmployeeManagement.Application.DTOs.Authentication;
using EmployeeManagement.Domain.Entities;
using System.Security.Claims;

namespace EmployeeManagement.Application.Interfaces.Services;

public interface IJwtService
{
    AccessTokenResultDto GenerateAccessToken(ApplicationUser user, IList<string> roles, IList<Claim> effectiveClaims);
}