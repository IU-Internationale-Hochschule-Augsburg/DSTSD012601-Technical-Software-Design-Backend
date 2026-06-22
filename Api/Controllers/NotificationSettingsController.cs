using Microsoft.AspNetCore.Mvc;
using Subscription_Control_Backend.Api.Mapper;
using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.NotificationSettings;

namespace Subscription_Control_Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationSettingsController : ControllerBase
{
    private readonly INotificationSettingsService _service;

    public NotificationSettingsController(INotificationSettingsService service)
    {
        _service = service;
    }

    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken ct)
    {
        var result = await _service.GetByUserIdAsync(userId, ct);
        return result is null ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Notification Settings nicht gefunden.")) : Ok(ApiResponseMapper.ToResponse(true, result, "Notification Settings geladen.", string.Empty));
    }

    [HttpPut("by-user/{userId:guid}")]
    public async Task<IActionResult> UpdateByUserId(Guid userId, [FromBody] UpdateNotificationSettingsRequest request, CancellationToken ct)
    {
        var result = await _service.UpdateByUserIdAsync(userId, request, ct);
        return result is null ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Notification Settings nicht gefunden.")) : Ok(ApiResponseMapper.ToResponse(true, result, "Notification Settings aktualisiert.", string.Empty));
    }
}
