namespace Subscription_Control_Backend.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    /// <summary>SHA-256-Hash des Refresh-Tokens. Der Klartext wird nur dem Client ausgehändigt, nie gespeichert.</summary>
    public string TokenHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    /// <summary>Hash des Nachfolge-Tokens nach einer Rotation (für Audit/Nachvollziehbarkeit).</summary>
    public string? ReplacedByTokenHash { get; set; }

    public User? User { get; set; }

    public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;
}