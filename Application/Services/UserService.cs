using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.Users;
using Subscription_Control_Backend.Contracts.Responses.Users;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Mapper;

namespace Subscription_Control_Backend.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<UserResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await _repository.GetAllAsync(ct);
        return users.Select(UserMapper.ToResponse).ToList();
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        return user is null ? null : UserMapper.ToResponse(user);
    }

    public Task<UserResponse?> RequestUser(Guid uuid, CancellationToken ct = default) => GetByIdAsync(uuid, ct);

    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        var user = UserMapper.ToEntity(request, PasswordHashService.Hash(request.Password));
        await _repository.AddAsync(user, ct);
        var created = await _repository.GetByIdAsync(user.Id, ct) ?? user;
        return UserMapper.ToResponse(created);
    }

    public async Task<UserResponse?> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct = default)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        if (user is null)
        {
            return null;
        }

        var passwordHash = string.IsNullOrWhiteSpace(request.Password) ? null : PasswordHashService.Hash(request.Password);
        UserMapper.UpdateEntity(user, request, passwordHash);
        await _repository.UpdateAsync(user, ct);
        var updated = await _repository.GetByIdAsync(id, ct) ?? user;
        return UserMapper.ToResponse(updated);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default) => _repository.DeleteAsync(id, ct);
}
