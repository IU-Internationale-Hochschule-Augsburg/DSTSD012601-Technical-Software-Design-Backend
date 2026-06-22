namespace Subscription_Control_Backend.Contracts.Requests.Users;

public class UpdateUserRequest
{
    public string? Password { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Timezone { get; set; } = "Europe/Berlin";
}
