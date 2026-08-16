namespace EmployeeManagement.Application.DTOs.Authentication;

public class AuthResponseDto
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public string? AccessToken { get; set; }
    public DateTime? AccessTokenExpiresAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
    
}