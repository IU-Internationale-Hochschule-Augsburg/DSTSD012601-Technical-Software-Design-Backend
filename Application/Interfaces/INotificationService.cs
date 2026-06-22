using Subscription_Control_Backend.Contracts.Requests.Notifications;
using Subscription_Control_Backend.Contracts.Responses.Notifications;

namespace Subscription_Control_Backend.Application.Interfaces;

public interface INotificationService : ICrudService<NotificationResponse, CreateNotificationRequest, UpdateNotificationRequest>
{
}
