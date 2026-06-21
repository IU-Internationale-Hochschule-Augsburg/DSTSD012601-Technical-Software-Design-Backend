namespace Subscription_Control_Backend.Contracts.Responses.NotificationSettings;

public class NotificationSettingsResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool Enabled { get; set; }
    public int ReminderDaysBefore { get; set; }
}
