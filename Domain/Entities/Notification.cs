using Subscription_Control_Backend.Domain.Enums;

namespace Subscription_Control_Backend.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
    public NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime ScheduledFor { get; set; }
}
