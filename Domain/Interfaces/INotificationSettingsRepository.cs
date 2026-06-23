using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Domain.Interfaces;

public interface INotificationSettingsRepository : IRepository<NotificationSettings>
{
    Task<NotificationSettings?> GetByUserIdAsync(Guid userId, bool asNoTracking = false, CancellationToken ct = default);
}