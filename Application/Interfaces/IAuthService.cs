using Subscription_Control_Backend.Contracts.Requests.Auth;
using Subscription_Control_Backend.Contracts.Responses.Auth;

namespace Subscription_Control_Backend.Application.Interfaces;

public interface IAuthService
{
    /// <summary>Prüft die Credentials und gibt bei Erfolg Access- + Refresh-Token zurück. Sonst <c>null</c>.</summary>
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default);

    /// <summary>Rotiert einen gültigen Refresh-Token gegen ein neues Token-Paar. Bei ungültig/abgelaufen/revoked <c>null</c>.</summary>
    Task<AuthResponse?> RefreshAsync(RefreshRequest request, CancellationToken ct = default);

    /// <summary>Widerruft den angegebenen Refresh-Token (idempotent).</summary>
    Task LogoutAsync(RefreshRequest request, CancellationToken ct = default);
}