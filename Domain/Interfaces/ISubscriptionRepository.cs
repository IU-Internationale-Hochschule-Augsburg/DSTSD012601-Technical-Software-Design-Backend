using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Domain.Interfaces;

public interface ISubscriptionRepository : IRepository<Subscription>
{
    Task<List<Subscription>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
}
