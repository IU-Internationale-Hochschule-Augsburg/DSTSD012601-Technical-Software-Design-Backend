using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Subscription_Control_Backend.Api.Mapper;
using Subscription_Control_Backend.Application.Interfaces;

namespace Subscription_Control_Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExampleController : ControllerBase
{
    private readonly IExampleService _exampleService;

    public ExampleController(IExampleService exampleService)
    {
        _exampleService = exampleService;
    }
    
    [HttpGet("test")]
    public async Task<IActionResult> Test(CancellationToken cancellationToken)
    {
        return Ok(ApiResponseMapper.ToResponse(
            true,
            "test",
            "",
            ""));
    }
}