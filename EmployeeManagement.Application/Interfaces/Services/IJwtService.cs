using EmployeeManagement.Application.DTOs.Authentication;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces.Services;

public interface IJwtService
{
    AccessTokenResultDto GenerateAccessToken(ApplicationUser user);
}