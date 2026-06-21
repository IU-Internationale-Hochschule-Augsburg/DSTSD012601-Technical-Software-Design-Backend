using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
}
