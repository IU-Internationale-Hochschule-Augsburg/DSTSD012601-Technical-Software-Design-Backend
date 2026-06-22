using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Subscription_Control_Backend.Api.Mapper;
using Subscription_Control_Backend.Application.Interfaces;

namespace Subscription_Control_Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Health(CancellationToken cancellationToken)
    {
        return Ok(ApiResponseMapper.ToResponse(
            true,
            "",
            "I'm still alive. Don't touch me, you fool!",
            ""));
    }
}