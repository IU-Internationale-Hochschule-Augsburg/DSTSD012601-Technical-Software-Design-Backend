using Microsoft.AspNetCore.Mvc;
using Subscription_Control_Backend.Api.Mapper;
using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.Users;

namespace Subscription_Control_Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var users = await _service.GetAllAsync(ct);
        return Ok(ApiResponseMapper.ToResponse(true, users, "Users geladen.", string.Empty));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var user = await _service.GetByIdAsync(id, ct);
        return user is null
            ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "User nicht gefunden."))
            : Ok(ApiResponseMapper.ToResponse(true, user, "User geladen.", string.Empty));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var user = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, ApiResponseMapper.ToResponse(true, user, "User erstellt.", string.Empty));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request, CancellationToken ct)
    {
        var user = await _service.UpdateAsync(id, request, ct);
        return user is null
            ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "User nicht gefunden."))
            : Ok(ApiResponseMapper.ToResponse(true, user, "User aktualisiert.", string.Empty));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(id, ct);
        return deleted
            ? Ok(ApiResponseMapper.ToResponse(true, id, "User gelöscht.", string.Empty))
            : NotFound(ApiResponseMapper.ToResponse(false, id, string.Empty, "User nicht gefunden."));
    }
}
