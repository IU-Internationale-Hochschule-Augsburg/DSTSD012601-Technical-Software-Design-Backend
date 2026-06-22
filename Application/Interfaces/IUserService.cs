using Subscription_Control_Backend.Application.Results;
using Subscription_Control_Backend.Contracts.Requests.Users;
using Subscription_Control_Backend.Contracts.Responses.Users;

namespace Subscription_Control_Backend.Application.Interfaces;

public interface IUserService : ICrudService<UserResponse, CreateUserRequest, UpdateUserRequest>
{
    Task<UserResponse?> RequestUser(Guid uuid, CancellationToken ct = default);

    Task<RegistrationResult> RegisterAsync(CreateUserRequest request, CancellationToken ct = default);
}
