using Microsoft.EntityFrameworkCore;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Infrastructure.Persistence;

namespace Subscription_Control_Backend.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<List<User>> GetAllAsync(CancellationToken ct = default)
    {
        return await DbContext.Users
            .Include(x => x.NotificationSettings)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public override async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await DbContext.Users
            .Include(x => x.NotificationSettings)
            .Include(x => x.Subscriptions)
            .ThenInclude(x => x.Category)
            .Include(x => x.Subscriptions)
            .ThenInclude(x => x.BillingCycle)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await DbContext.Users
            .Include(x => x.NotificationSettings)
            .FirstOrDefaultAsync(x => x.Email == email.Trim().ToLowerInvariant(), ct);
    }
}
