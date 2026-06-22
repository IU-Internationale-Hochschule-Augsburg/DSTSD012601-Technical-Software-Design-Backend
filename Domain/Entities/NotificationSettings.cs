namespace Subscription_Control_Backend.Domain.Entities;

public class NotificationSettings
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public bool Enabled { get; set; } = true;
    public int ReminderDaysBefore { get; set; } = 7;
}
