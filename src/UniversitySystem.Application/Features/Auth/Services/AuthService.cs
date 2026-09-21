using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Features.Auth.Commands.Login;
using UniversitySystem.Application.Features.Auth.Repositories;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.Application.Features.Auth.Services;
/// <summary>
/// حساب فعال کاربر را پیدا می‌کند، رمز را بررسی می‌کند و با نقش‌های کاربر توکن ورود می‌سازد.
/// </summary>
public sealed class AuthService(IAuthRepository repository, IPasswordHasher passwordHasher, ITokenService tokenService) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new BusinessException("نام کاربری الزامی است.");
        }

        if (username.Length > 100)
        {
            throw new BusinessException("نام کاربری نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new BusinessException("کلمه عبور الزامی است.");
        }

        if (password.Length > 200)
        {
            throw new BusinessException("کلمه عبور نمی‌تواند بیشتر از ۲۰۰ کاراکتر باشد.");
        }

        var normalizedUsername = username.Trim();
        var user = await repository.GetUserByUsernameAsync(normalizedUsername, cancellationToken);
        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("نام کاربری یا کلمه عبور نادرست است.");
        }

        if (!passwordHasher.Verify(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("نام کاربری یا کلمه عبور نادرست است.");
        }

        var roleNames = await repository.GetUserRolesAsync(user.Id, cancellationToken);
        var(accessToken, expiresAt) = tokenService.GenerateToken(user, roleNames);
        return new LoginResponse
        {
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            UserId = user.Id,
            FullName = UserLogic.GetFullName(user),
            Roles = roleNames
        };
    }
}

