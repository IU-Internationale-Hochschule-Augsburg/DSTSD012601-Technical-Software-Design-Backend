using Subscription_Control_Backend.Contracts.Responses.Users;

namespace Subscription_Control_Backend.Application.Results;

public enum RegistrationStatus
{
    Success,
    EmailAlreadyExists
}

public record RegistrationResult(RegistrationStatus Status, UserResponse? User)
{
    public static RegistrationResult Success(UserResponse user) => new(RegistrationStatus.Success, user);

    public static RegistrationResult EmailAlreadyExists() => new(RegistrationStatus.EmailAlreadyExists, null);
}