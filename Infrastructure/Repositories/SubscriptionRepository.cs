using Microsoft.EntityFrameworkCore;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Infrastructure.Persistence;

namespace Subscription_Control_Backend.Infrastructure.Repositories;

public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<List<Subscription>> GetAllAsync(CancellationToken ct = default)
    {
        return await QueryWithIncludes().AsNoTracking().ToListAsync(ct);
    }

    public override async Task<Subscription?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await QueryWithIncludes().FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<Subscription>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await QueryWithIncludes().Where(x => x.UserId == userId).AsNoTracking().ToListAsync(ct);
    }

    private IQueryable<Subscription> QueryWithIncludes()
    {
        return DbContext.Subscriptions
            .Include(x => x.Category)
            .Include(x => x.BillingCycle)
            .Include(x => x.Notifications);
    }
}
