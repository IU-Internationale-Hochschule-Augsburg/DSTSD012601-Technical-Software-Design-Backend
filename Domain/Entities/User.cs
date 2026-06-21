namespace Subscription_Control_Backend.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Timezone { get; set; } = "Europe/Berlin";

    public NotificationSettings? NotificationSettings { get; set; }
    public List<Subscription> Subscriptions { get; set; } = [];
}
