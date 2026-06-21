namespace Subscription_Control_Backend.Contracts.Requests.Users;

public class CreateUserRequest
{
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Timezone { get; set; } = "Europe/Berlin";
    public bool NotificationsEnabled { get; set; } = true;
    public int ReminderDaysBefore { get; set; } = 7;
}
