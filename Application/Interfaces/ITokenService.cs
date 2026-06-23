using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Application.Interfaces;

public interface ITokenService
{
    /// <summary>Erzeugt ein signiertes Access-JWT für den User.</summary>
    (string Token, DateTime ExpiresAt) CreateAccessToken(User user);

    /// <summary>Erzeugt einen opaken Refresh-Token: Klartext (für den Client), dessen Hash (für die DB) und Ablauf.</summary>
    (string Token, string TokenHash, DateTime ExpiresAt) CreateRefreshToken();

    /// <summary>Berechnet den deterministischen Hash eines Refresh-Tokens für den DB-Abgleich.</summary>
    string HashToken(string token);
}