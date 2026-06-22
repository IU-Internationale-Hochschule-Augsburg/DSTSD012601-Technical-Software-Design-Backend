using Subscription_Control_Backend.Domain.Enums;

namespace Subscription_Control_Backend.Contracts.Requests.Notifications;

public class UpdateNotificationRequest
{
    public Guid SubscriptionId { get; set; }
    public NotificationType Type { get; set; } = NotificationType.PaymentReminder;
    public string Message { get; set; } = string.Empty;
    public DateTime ScheduledFor { get; set; }
}
