namespace EnterpriseIdentity_Auth.Domain.Entities;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Expires { get; set; }

    public DateTime Created { get; set; }

    public string CreatedByIp { get; set; } = null!;

    public DateTime? Revoked { get; set; }

    public string? RevokedByIp { get; set; }

    public string? ReplacedByToken { get; set; }

    public string? DeviceType { get; set; }

    public string? Browser { get; set; }

    public string? OperatingSystem { get; set; }

    public string? UserAgent { get; set; }

    public DateTime? LastActivity { get; set; }

    public Guid? SessionId { get; set; }

    public virtual User User { get; set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= Expires;

    public bool IsActive => Revoked == null && !IsExpired;

}
