using Microsoft.EntityFrameworkCore;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Infrastructure.Persistence;

namespace Subscription_Control_Backend.Infrastructure.Repositories;

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default)
    {
        return await DbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);
    }
}