using Subscription_Control_Backend.Contracts.Requests.Subscriptions;
using Subscription_Control_Backend.Contracts.Responses.Subscriptions;

namespace Subscription_Control_Backend.Application.Interfaces;

public interface ISubscriptionService : ICrudService<SubscriptionResponse, CreateSubscriptionRequest, UpdateSubscriptionRequest>
{
    Task<List<SubscriptionResponse>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
}
