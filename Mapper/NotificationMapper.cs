using Subscription_Control_Backend.Contracts.Requests.Notifications;
using Subscription_Control_Backend.Contracts.Responses.Notifications;
using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Mapper;

public static class NotificationMapper
{
    public static NotificationResponse ToResponse(Notification notification) => new()
    {
        Id = notification.Id,
        SubscriptionId = notification.SubscriptionId,
        Type = notification.Type,
        Message = notification.Message,
        ScheduledFor = notification.ScheduledFor
    };

    public static Notification ToEntity(CreateNotificationRequest request) => new()
    {
        Id = Guid.NewGuid(),
        SubscriptionId = request.SubscriptionId,
        Type = request.Type,
        Message = request.Message.Trim(),
        ScheduledFor = request.ScheduledFor
    };

    public static void UpdateEntity(Notification notification, UpdateNotificationRequest request)
    {
        notification.SubscriptionId = request.SubscriptionId;
        notification.Type = request.Type;
        notification.Message = request.Message.Trim();
        notification.ScheduledFor = request.ScheduledFor;
    }
}
