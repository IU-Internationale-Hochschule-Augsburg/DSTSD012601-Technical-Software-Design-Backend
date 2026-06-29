using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Subscription_Control_Backend.Api.Mapper;
using Subscription_Control_Backend.Api.Response_Wrapper;
using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Application.Results;
using Subscription_Control_Backend.Contracts.Requests.Auth;
using Subscription_Control_Backend.Contracts.Requests.Users;
using Subscription_Control_Backend.Contracts.Responses.Auth;
using Subscription_Control_Backend.Contracts.Responses.Users;

namespace Subscription_Control_Backend.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
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

        var result = await _userService.RegisterAsync(request, ct);

        if (result.Status == RegistrationStatus.EmailAlreadyExists)
        {
            return TypedResults.Conflict(ApiResponseMapper.ToResponse(
                false, (object?)null, string.Empty, "Es existiert bereits ein User mit dieser Email."));
        }

        var user = result.User!;
        return TypedResults.Created($"/api/users/{user.Id}",
            ApiResponseMapper.ToResponse(true, user, "User registriert.", string.Empty));
    }

    [HttpPost("login")]
    public async Task<Results<
        Ok<ApiResponse<AuthResponse>>,
        BadRequest<ApiResponse<object?>>>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return TypedResults.BadRequest(ApiResponseMapper.ToResponse(
                false, (object?)null, string.Empty, "Email und Passwort sind erforderlich."));
        }

        var auth = await _authService.LoginAsync(request, ct);
        if (auth is null)
        {
            // Bewusst nicht unterscheiden, ob Email unbekannt oder Passwort falsch ist.
            return TypedResults.BadRequest(ApiResponseMapper.ToResponse(
                false, (object?)null, string.Empty, "Email oder Passwort ist falsch."));
        }

        return TypedResults.Ok(ApiResponseMapper.ToResponse(true, auth, "Login erfolgreich.", string.Empty));
    }

    [HttpPost("refresh")]
    public async Task<Results<
        Ok<ApiResponse<AuthResponse>>,
        BadRequest<ApiResponse<object?>>>> Refresh([FromBody] RefreshRequest request, CancellationToken ct)
    {
        var auth = await _authService.RefreshAsync(request, ct);
        if (auth is null)
        {
            return TypedResults.BadRequest(ApiResponseMapper.ToResponse(
                false, (object?)null, string.Empty, "Refresh-Token ist ungültig oder abgelaufen."));
        }

        return TypedResults.Ok(ApiResponseMapper.ToResponse(true, auth, "Token erneuert.", string.Empty));
    }

    [HttpPost("logout")]
    public async Task<Ok<ApiResponse<object?>>> Logout([FromBody] RefreshRequest request, CancellationToken ct)
    {
        await _authService.LogoutAsync(request, ct);
        return TypedResults.Ok(ApiResponseMapper.ToResponse(
            true, (object?)null, "Logout erfolgreich.", string.Empty));
    }
}