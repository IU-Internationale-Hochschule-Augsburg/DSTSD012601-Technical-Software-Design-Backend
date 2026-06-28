using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Domain.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default);
}