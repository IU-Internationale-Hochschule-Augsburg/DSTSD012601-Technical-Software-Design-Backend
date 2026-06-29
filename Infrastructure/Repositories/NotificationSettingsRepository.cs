using Microsoft.EntityFrameworkCore;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Infrastructure.Persistence;

namespace Subscription_Control_Backend.Infrastructure.Repositories;

public class NotificationSettingsRepository : Repository<NotificationSettings>, INotificationSettingsRepository
{
    public NotificationSettingsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<NotificationSettings?> GetByUserIdAsync(Guid userId, bool asNoTracking = false, CancellationToken ct = default)
    {
        var query = DbContext.NotificationSettings.AsQueryable();
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(x => x.UserId == userId, ct);
    }
}