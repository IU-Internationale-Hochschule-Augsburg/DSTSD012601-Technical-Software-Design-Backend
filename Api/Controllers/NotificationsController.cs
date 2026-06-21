using Microsoft.AspNetCore.Mvc;
using Subscription_Control_Backend.Api.Mapper;
using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.Notifications;

namespace Subscription_Control_Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(ApiResponseMapper.ToResponse(true, await _service.GetAllAsync(ct), "Notifications geladen.", string.Empty));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Notification nicht gefunden.")) : Ok(ApiResponseMapper.ToResponse(true, result, "Notification geladen.", string.Empty));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request, CancellationToken ct)
    {
        var result = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponseMapper.ToResponse(true, result, "Notification erstellt.", string.Empty));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNotificationRequest request, CancellationToken ct)
    {
        var result = await _service.UpdateAsync(id, request, ct);
        return result is null ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Notification nicht gefunden.")) : Ok(ApiResponseMapper.ToResponse(true, result, "Notification aktualisiert.", string.Empty));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(id, ct);
        return deleted ? Ok(ApiResponseMapper.ToResponse(true, id, "Notification gelöscht.", string.Empty)) : NotFound(ApiResponseMapper.ToResponse(false, id, string.Empty, "Notification nicht gefunden."));
    }
}
