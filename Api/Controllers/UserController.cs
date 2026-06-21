using Microsoft.AspNetCore.Mvc;
using Subscription_Control_Backend.Api.Mapper;
using Subscription_Control_Backend.Application.Interfaces;

namespace Subscription_Control_Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> RequestUser(Guid uuid, CancellationToken ct)
    {
        var user = await _service.RequestUser(uuid, ct);
        return user is null
            ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "User nicht gefunden."))
            : Ok(ApiResponseMapper.ToResponse(true, user, "User geladen.", string.Empty));
    }
}
