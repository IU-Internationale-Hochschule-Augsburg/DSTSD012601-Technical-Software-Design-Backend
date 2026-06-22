using Subscription_Control_Backend.Contracts.Responses.NotificationSettings;

namespace Subscription_Control_Backend.Contracts.Responses.Users;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Timezone { get; set; } = string.Empty;
    public NotificationSettingsResponse? NotificationSettings { get; set; }
}
