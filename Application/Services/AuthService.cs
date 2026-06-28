using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.Auth;
using Subscription_Control_Backend.Contracts.Responses.Auth;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Mapper;

namespace Subscription_Control_Backend.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (user is null || !PasswordHashService.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        return await IssueTokensAsync(user, ct);
    }

    public async Task<AuthResponse?> RefreshAsync(RefreshRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return null;
        }

        var stored = await _refreshTokenRepository.GetByTokenHashAsync(_tokenService.HashToken(request.RefreshToken), ct);
        if (stored is null || !stored.IsActive)
        {
            return null;
        }

        var user = await _userRepository.GetByIdAsync(stored.UserId, ct);
        if (user is null)
        {
            return null;
        }

        var auth = await IssueTokensAsync(user, ct, replaces: stored);
        return auth;
    }

    public async Task LogoutAsync(RefreshRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return;
        }

        var stored = await _refreshTokenRepository.GetByTokenHashAsync(_tokenService.HashToken(request.RefreshToken), ct);
        if (stored is null || stored.RevokedAt is not null)
        {
            return;
        }

        stored.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(stored, ct);
    }

    private async Task<AuthResponse> IssueTokensAsync(User user, CancellationToken ct, RefreshToken? replaces = null)
    {
        var (accessToken, accessExpiresAt) = _tokenService.CreateAccessToken(user);
        var (refreshToken, refreshHash, refreshExpiresAt) = _tokenService.CreateRefreshToken();

        if (replaces is not null)
        {
            // Rotation: alten Token widerrufen und auf den Nachfolger verweisen.
            replaces.RevokedAt = DateTime.UtcNow;
            replaces.ReplacedByTokenHash = refreshHash;
            await _refreshTokenRepository.UpdateAsync(replaces, ct);
        }

        await _refreshTokenRepository.AddAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = refreshHash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = refreshExpiresAt
        }, ct);

        return new AuthResponse
        {
            AccessToken = accessToken,
            AccessTokenExpiresAt = accessExpiresAt,
            RefreshToken = refreshToken,
            User = UserMapper.ToResponse(user)
        };
    }
}