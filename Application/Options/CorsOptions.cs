namespace Subscription_Control_Backend.Application.Options;

public class CorsOptions
{
    public const string SectionName = "Cors";

    public string[] AllowedOrigins { get; set; } = [];
}