using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.Notifications;
using Subscription_Control_Backend.Contracts.Responses.Notifications;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Mapper;

namespace Subscription_Control_Backend.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _repository;

    public NotificationService(IRepository<Notification> repository)
    {
        _repository = repository;
    }

    public async Task<List<NotificationResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var notifications = await _repository.GetAllAsync(ct);
        return notifications.Select(NotificationMapper.ToResponse).ToList();
    }

    public async Task<NotificationResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var notification = await _repository.GetByIdAsync(id, ct);
        return notification is null ? null : NotificationMapper.ToResponse(notification);
    }

    public async Task<NotificationResponse> CreateAsync(CreateNotificationRequest request, CancellationToken ct = default)
    {
        var notification = NotificationMapper.ToEntity(request);
        await _repository.AddAsync(notification, ct);
        return NotificationMapper.ToResponse(notification);
    }

    public async Task<NotificationResponse?> UpdateAsync(Guid id, UpdateNotificationRequest request, CancellationToken ct = default)
    {
        var notification = await _repository.GetByIdAsync(id, ct);
        if (notification is null)
        {
            return null;
        }

        NotificationMapper.UpdateEntity(notification, request);
        await _repository.UpdateAsync(notification, ct);
        return NotificationMapper.ToResponse(notification);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default) => _repository.DeleteAsync(id, ct);
}
