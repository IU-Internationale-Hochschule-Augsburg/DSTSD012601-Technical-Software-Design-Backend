using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.Subscriptions;
using Subscription_Control_Backend.Contracts.Responses.Subscriptions;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Mapper;

namespace Subscription_Control_Backend.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _repository;

    public SubscriptionService(ISubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SubscriptionResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var subscriptions = await _repository.GetAllAsync(ct);
        return subscriptions.Select(SubscriptionMapper.ToResponse).ToList();
    }

    public async Task<SubscriptionResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var subscription = await _repository.GetByIdAsync(id, ct);
        return subscription is null ? null : SubscriptionMapper.ToResponse(subscription);
    }

    public async Task<List<SubscriptionResponse>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var subscriptions = await _repository.GetByUserIdAsync(userId, ct);
        return subscriptions.Select(SubscriptionMapper.ToResponse).ToList();
    }

    public async Task<SubscriptionResponse> CreateAsync(CreateSubscriptionRequest request, CancellationToken ct = default)
    {
        var subscription = SubscriptionMapper.ToEntity(request);
        await _repository.AddAsync(subscription, ct);
        var created = await _repository.GetByIdAsync(subscription.Id, ct) ?? subscription;
        return SubscriptionMapper.ToResponse(created);
    }

    public async Task<SubscriptionResponse?> UpdateAsync(Guid id, UpdateSubscriptionRequest request, CancellationToken ct = default)
    {
        var subscription = await _repository.GetByIdAsync(id, ct);
        if (subscription is null)
        {
            return null;
        }

        SubscriptionMapper.UpdateEntity(subscription, request);
        await _repository.UpdateAsync(subscription, ct);
        var updated = await _repository.GetByIdAsync(id, ct) ?? subscription;
        return SubscriptionMapper.ToResponse(updated);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default) => _repository.DeleteAsync(id, ct);
}
