using Microsoft.EntityFrameworkCore;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Infrastructure.Persistence;

namespace Subscription_Control_Backend.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext DbContext;
    protected readonly DbSet<T> Set;

    public Repository(AppDbContext dbContext)
    {
        DbContext = dbContext;
        Set = dbContext.Set<T>();
    }

    public virtual async Task<List<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await Set.AsNoTracking().ToListAsync(ct);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await Set.FindAsync([id], ct);
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        Set.Add(entity);
        await DbContext.SaveChangesAsync(ct);
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken ct = default)
    {
        Set.Update(entity);
        await DbContext.SaveChangesAsync(ct);
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await Set.FindAsync([id], ct);
        if (entity is null)
        {
            return false;
        }

        Set.Remove(entity);
        await DbContext.SaveChangesAsync(ct);
        return true;
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        return await Set.FindAsync([id], ct) is not null;
    }
}
