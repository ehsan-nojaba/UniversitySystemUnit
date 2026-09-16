using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.Auth.Commands.Login;
using UniversitySystem.Application.Features.Auth.Repositories;

namespace UniversitySystem.Application.Features.Auth.Services;

/// <summary>
/// حساب فعال کاربر را پیدا می‌کند، رمز را بررسی می‌کند و با نقش‌های کاربر توکن ورود می‌سازد.
/// </summary>
public sealed class AuthService(IAuthRepository repository, IPasswordHasher passwordHasher, ITokenService tokenService) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim();
        var user = await repository.GetUserByUsernameAsync(normalizedUsername, cancellationToken);
        if (user is null || !user.IsActive)
            throw new UnauthorizedAccessException("نام کاربری یا کلمه عبور نادرست است.");

        if (!passwordHasher.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("نام کاربری یا کلمه عبور نادرست است.");

        var roleNames = await repository.GetUserRolesAsync(user.Id, cancellationToken);
        var (accessToken, expiresAt) = tokenService.GenerateToken(user, roleNames);

        return new LoginResponse
        {
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            UserId = user.Id,
            FullName = user.FullName,
            Roles = roleNames
        };
    }
}
