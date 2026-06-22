using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Subscription_Control_Backend.Api.Mapper;
using Subscription_Control_Backend.Api.Response_Wrapper;
using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Application.Results;
using Subscription_Control_Backend.Contracts.Requests.Users;
using Subscription_Control_Backend.Contracts.Responses.Users;

namespace Subscription_Control_Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;

    public AuthController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<Results<
        Created<ApiResponse<UserResponse>>,
        Conflict<ApiResponse<object?>>,
        BadRequest<ApiResponse<object?>>>> Register([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.Name))
        {
            return TypedResults.BadRequest(ApiResponseMapper.ToResponse(
                false, (object?)null, string.Empty, "Email, Passwort und Name sind erforderlich."));
        }

        var result = await _service.RegisterAsync(request, ct);

        if (result.Status == RegistrationStatus.EmailAlreadyExists)
        {
            return TypedResults.Conflict(ApiResponseMapper.ToResponse(
                false, (object?)null, string.Empty, "Es existiert bereits ein User mit dieser Email."));
        }

        var user = result.User!;
        return TypedResults.Created($"/api/users/{user.Id}",
            ApiResponseMapper.ToResponse(true, user, "User registriert.", string.Empty));
    }
}