using Subscription_Control_Backend.Contracts.Requests.NotificationSettings;
using Subscription_Control_Backend.Contracts.Responses.NotificationSettings;

namespace Subscription_Control_Backend.Application.Interfaces;

public interface INotificationSettingsService
{
    Task<NotificationSettingsResponse?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<NotificationSettingsResponse?> UpdateByUserIdAsync(Guid userId, UpdateNotificationSettingsRequest request, CancellationToken ct = default);
}
