namespace Subscription_Control_Backend.Api.Response_Wrapper;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public required T Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}