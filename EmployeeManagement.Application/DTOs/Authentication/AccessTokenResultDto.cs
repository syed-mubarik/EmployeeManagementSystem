namespace EmployeeManagement.Application.DTOs.Authentication;

public class AccessTokenResultDto
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}