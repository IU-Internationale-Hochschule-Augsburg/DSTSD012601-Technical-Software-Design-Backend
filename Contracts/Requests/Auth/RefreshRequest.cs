namespace Subscription_Control_Backend.Contracts.Requests.Auth;

public class RefreshRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}