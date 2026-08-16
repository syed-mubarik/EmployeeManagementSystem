using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;
public class RefreshToken : BaseEntity
{
    public string TokenHash { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? ReplacedByTokenHash { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}
