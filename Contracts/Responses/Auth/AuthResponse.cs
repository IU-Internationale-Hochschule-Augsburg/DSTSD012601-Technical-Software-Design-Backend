using Subscription_Control_Backend.Contracts.Responses.Users;

namespace Subscription_Control_Backend.Contracts.Responses.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiresAt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public UserResponse User { get; set; } = new();
}