using AutoMapper;
using EmployeeManagement.Application.DTOs.Authentication;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Application.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUnitOfWork _unitOfWork;
    public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper,
                       IJwtService jwtService, IRefreshTokenService refreshTokenService,
                       IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var email = request.Email.Trim();
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = "Email already exists."
            };
        }
        request.Email = email;
        var user = _mapper.Map<ApplicationUser>(request);

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = "User registration failed. Please correct the validation errors and try again.",
                Errors = result.Errors
                                .Select(e => e.Description)
                                .ToList()
            };
        }
        return new AuthResponseDto
        {
            IsSuccess = true,
            Message = "User registered Successfully"
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = "Invalid email or password."
            };
        }
        var passwordValid = await _userManager.CheckPasswordAsync(
            user,
            request.Password);
        if (!passwordValid)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = "Invalid email or password."
            };
        }
        var accessTokenResult = _jwtService.GenerateAccessToken(user);

        var refreshToken = _refreshTokenService.GenerateToken();

        var refreshTokenHash = _refreshTokenService.HashToken(refreshToken);
        // create the entity:
        var refreshTokenExpiresAt = _refreshTokenService.GetExpirationTime();

        var refreshTokenEntity = new RefreshToken
        {
            TokenHash = refreshTokenHash,
            ExpiresAt = refreshTokenExpiresAt,
            UserId = user.Id
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponseDto
        {
            IsSuccess = true,
            Message = "Login successful.",
            AccessToken = accessTokenResult.AccessToken,
            AccessTokenExpiresAt = accessTokenResult.ExpiresAt,
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var tokenHash = _refreshTokenService.HashToken(request.RefreshToken);

        var storedToken =   await _unitOfWork.RefreshTokens.GetByTokenHashAsync(tokenHash);

        if (storedToken == null)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = "Invalid refresh token."
            };
        }

        if (storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = "Refresh token has expired."
            };
        }

        if (storedToken.RevokedAt.HasValue)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = "Refresh token has been revoked."
            };
        }

        var user = storedToken.User;

        var accessTokenResult = _jwtService.GenerateAccessToken(user);

        var newRefreshToken =   _refreshTokenService.GenerateToken();

        var newRefreshTokenHash = _refreshTokenService.HashToken(newRefreshToken);

        var newRefreshTokenExpiresAt =  _refreshTokenService.GetExpirationTime();

        var newRefreshTokenEntity = new RefreshToken
        {
            TokenHash = newRefreshTokenHash,
            ExpiresAt = newRefreshTokenExpiresAt,
            UserId = user.Id
        };

        storedToken.RevokedAt = DateTime.UtcNow;
        storedToken.ReplacedByTokenHash =   newRefreshTokenHash;

        _unitOfWork.RefreshTokens.Update(storedToken);

        await _unitOfWork.RefreshTokens.AddAsync(newRefreshTokenEntity);

        await _unitOfWork.SaveChangesAsync();

        return new AuthResponseDto
        {
            IsSuccess = true,
            Message = "Token refreshed successfully.",
            AccessToken = accessTokenResult.AccessToken,
            AccessTokenExpiresAt = accessTokenResult.ExpiresAt,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiresAt = newRefreshTokenExpiresAt
        };
    }

    public async Task<AuthResponseDto> RevokeRefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var tokenHash = _refreshTokenService.HashToken(request.RefreshToken);

        var storedToken =   await _unitOfWork.RefreshTokens.GetByTokenHashAsync(tokenHash);

        if (storedToken == null)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = "Invalid refresh token."
            };
        }

        if (storedToken.RevokedAt.HasValue)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = "Refresh token has already been revoked."
            };
        }

        storedToken.RevokedAt = DateTime.UtcNow;

        _unitOfWork.RefreshTokens.Update(storedToken);

        await _unitOfWork.SaveChangesAsync();

        return new AuthResponseDto
        {
            IsSuccess = true,
            Message = "Refresh token revoked successfully."
        };
    }
}