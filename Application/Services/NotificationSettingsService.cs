using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.NotificationSettings;
using Subscription_Control_Backend.Contracts.Responses.NotificationSettings;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Mapper;

namespace Subscription_Control_Backend.Application.Services;

public class NotificationSettingsService : INotificationSettingsService
{
    private readonly INotificationSettingsRepository _repository;

    public NotificationSettingsService(INotificationSettingsRepository repository)
    {
        _repository = repository;
    }

    public async Task<NotificationSettingsResponse?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var settings = await _repository.GetByUserIdAsync(userId, asNoTracking: true, ct);
        return settings is null ? null : NotificationSettingsMapper.ToResponse(settings);
    }

    public async Task<NotificationSettingsResponse?> UpdateByUserIdAsync(Guid userId, UpdateNotificationSettingsRequest request, CancellationToken ct = default)
    {
        var settings = await _repository.GetByUserIdAsync(userId, ct: ct);
        if (settings is null)
        {
            return null;
        }

        NotificationSettingsMapper.UpdateEntity(settings, request);
        await _repository.UpdateAsync(settings, ct);
        return NotificationSettingsMapper.ToResponse(settings);
    }
}