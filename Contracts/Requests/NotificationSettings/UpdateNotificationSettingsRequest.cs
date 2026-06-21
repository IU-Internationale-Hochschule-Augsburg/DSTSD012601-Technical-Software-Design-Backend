namespace Subscription_Control_Backend.Contracts.Requests.NotificationSettings;

public class UpdateNotificationSettingsRequest
{
    public bool Enabled { get; set; } = true;
    public int ReminderDaysBefore { get; set; } = 7;
}
