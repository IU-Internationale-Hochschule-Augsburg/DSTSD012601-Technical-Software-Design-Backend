using Subscription_Control_Backend.Contracts.Requests.Users;
using Subscription_Control_Backend.Contracts.Responses.Users;
using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Mapper;

public static class UserMapper
{
    public static UserResponse ToResponse(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        Name = user.Name,
        Timezone = user.Timezone,
        NotificationSettings = user.NotificationSettings is null
            ? null
            : NotificationSettingsMapper.ToResponse(user.NotificationSettings)
    };

    public static User ToEntity(CreateUserRequest request, string passwordHash)
    {
        var userId = Guid.NewGuid();

        return new User
        {
            Id = userId,
            Email = request.Email.Trim().ToLowerInvariant(),
            Name = request.Name.Trim(),
            PasswordHash = passwordHash,
            Timezone = request.Timezone.Trim(),
            NotificationSettings = new NotificationSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Enabled = request.NotificationsEnabled,
                ReminderDaysBefore = request.ReminderDaysBefore
            }
        };
    }

    public static void UpdateEntity(User user, UpdateUserRequest request, string? passwordHash)
    {
        user.Email = request.Email.Trim().ToLowerInvariant();
        user.Name = request.Name.Trim();
        user.Timezone = request.Timezone.Trim();

        if (!string.IsNullOrWhiteSpace(passwordHash))
        {
            user.PasswordHash = passwordHash;
        }
    }
}
