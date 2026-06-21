using Subscription_Control_Backend.Domain.Enums;

namespace Subscription_Control_Backend.Contracts.Responses.Notifications;

public class NotificationResponse
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime ScheduledFor { get; set; }
}
