using EmployeeManagement.Application.DTOs.Authentication;

namespace EmployeeManagement.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<AuthResponseDto> RevokeRefreshTokenAsync(RefreshTokenRequestDto request);
}