using Subscription_Control_Backend.Api.Response_Wrapper;

namespace Subscription_Control_Backend.Api.Mapper;

public class ApiResponseMapper
{
    public static ApiResponse<T> ToResponse<T>(bool success, T data, string message, string errorMessage)
    {
        return new ApiResponse<T>
        {
            Success = success,
            Data = data,
            Message = message,
            ErrorMessage = errorMessage
        };
    }   
}