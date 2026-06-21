using Subscription_Control_Backend.Contracts.Requests.NotificationSettings;
using Subscription_Control_Backend.Contracts.Responses.NotificationSettings;
using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Mapper;

public static class NotificationSettingsMapper
{
    public static NotificationSettingsResponse ToResponse(NotificationSettings notificationSettings) => new()
    {
        Id = notificationSettings.Id,
        UserId = notificationSettings.UserId,
        Enabled = notificationSettings.Enabled,
        ReminderDaysBefore = notificationSettings.ReminderDaysBefore
    };

    public static void UpdateEntity(NotificationSettings notificationSettings, UpdateNotificationSettingsRequest request)
    {
        notificationSettings.Enabled = request.Enabled;
        notificationSettings.ReminderDaysBefore = request.ReminderDaysBefore;
    }
}
