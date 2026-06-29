using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Subscription_Control_Backend.Api.Mapper;
using Subscription_Control_Backend.Api.Response_Wrapper;
using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.Subscriptions;
using Subscription_Control_Backend.Contracts.Responses.Subscriptions;

namespace Subscription_Control_Backend.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _service;

    public SubscriptionsController(ISubscriptionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<Results<Ok<ApiResponse<List<SubscriptionResponse>>>, UnauthorizedHttpResult>> GetAll(CancellationToken ct)
    {
        if (!TryGetUserId(out var userId))
        {
            return TypedResults.Unauthorized();
        }

        var subscriptions = await _service.GetByUserIdAsync(userId, ct);
        return TypedResults.Ok(ApiResponseMapper.ToResponse(true, subscriptions, "Subscriptions geladen.", string.Empty));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Ungültiges Token."));
        }

        var result = await _service.GetByIdAsync(id, ct);
        // Fremde IDs werden wie nicht existent behandelt, um Existenz nicht zu verraten.
        return result is null || result.UserId != userId
            ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Subscription nicht gefunden."))
            : Ok(ApiResponseMapper.ToResponse(true, result, "Subscription geladen.", string.Empty));
    }

    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken ct)
    {
        if (!TryGetUserId(out var tokenUserId))
        {
            return Unauthorized(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Ungültiges Token."));
        }

        if (userId != tokenUserId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Zugriff auf fremde Subscriptions ist nicht erlaubt."));
        }

        return Ok(ApiResponseMapper.ToResponse(true, await _service.GetByUserIdAsync(tokenUserId, ct), "Subscriptions für User geladen.", string.Empty));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubscriptionRequest request, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Ungültiges Token."));
        }

        // UserId immer aus dem Token, ein im Body mitgeschickter Wert wird ignoriert.
        request.UserId = userId;
        var result = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponseMapper.ToResponse(true, result, "Subscription erstellt.", string.Empty));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSubscriptionRequest request, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Ungültiges Token."));
        }

        var existing = await _service.GetByIdAsync(id, ct);
        if (existing is null || existing.UserId != userId)
        {
            return NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Subscription nicht gefunden."));
        }

        // Ownership kann nicht auf einen anderen User umgehängt werden.
        request.UserId = userId;
        var result = await _service.UpdateAsync(id, request, ct);
        return result is null
            ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Subscription nicht gefunden."))
            : Ok(ApiResponseMapper.ToResponse(true, result, "Subscription aktualisiert.", string.Empty));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Ungültiges Token."));
        }

        var existing = await _service.GetByIdAsync(id, ct);
        if (existing is null || existing.UserId != userId)
        {
            return NotFound(ApiResponseMapper.ToResponse(false, id, string.Empty, "Subscription nicht gefunden."));
        }

        var deleted = await _service.DeleteAsync(id, ct);
        return deleted
            ? Ok(ApiResponseMapper.ToResponse(true, id, "Subscription gelöscht.", string.Empty))
            : NotFound(ApiResponseMapper.ToResponse(false, id, string.Empty, "Subscription nicht gefunden."));
    }

    private bool TryGetUserId(out Guid userId)
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(raw, out userId);
    }
}
