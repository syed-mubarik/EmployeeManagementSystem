using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Created(string.Empty, result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
       
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);

            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpPost("revoke")]
        public async Task<ActionResult<AuthResponseDto>> RevokeRefreshToken(RefreshTokenRequestDto dto)
        {
            var result = await _authService.RevokeRefreshTokenAsync(dto);

            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
