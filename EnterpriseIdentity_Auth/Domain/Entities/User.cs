namespace EnterpriseIdentity_Auth.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Username { get; set; }

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public int RolesId { get; set; }

    public bool IsActive { get; set; }

    public bool HasPassword { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public Role Roles { get; set; } = null!;
}
