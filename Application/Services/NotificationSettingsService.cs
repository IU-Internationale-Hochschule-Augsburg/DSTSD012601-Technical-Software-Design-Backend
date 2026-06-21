using Microsoft.EntityFrameworkCore;
using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.NotificationSettings;
using Subscription_Control_Backend.Contracts.Responses.NotificationSettings;
using Subscription_Control_Backend.Infrastructure.Persistence;
using Subscription_Control_Backend.Mapper;

namespace Subscription_Control_Backend.Application.Services;

public class NotificationSettingsService : INotificationSettingsService
{
    private readonly AppDbContext _dbContext;

    public NotificationSettingsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<NotificationSettingsResponse?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var settings = await _dbContext.NotificationSettings.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, ct);
        return settings is null ? null : NotificationSettingsMapper.ToResponse(settings);
    }

    public async Task<NotificationSettingsResponse?> UpdateByUserIdAsync(Guid userId, UpdateNotificationSettingsRequest request, CancellationToken ct = default)
    {
        var settings = await _dbContext.NotificationSettings.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        if (settings is null)
        {
            return null;
        }

        NotificationSettingsMapper.UpdateEntity(settings, request);
        await _dbContext.SaveChangesAsync(ct);
        return NotificationSettingsMapper.ToResponse(settings);
    }
}
