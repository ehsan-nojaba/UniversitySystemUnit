using MediatR;
using UniversitySystem.Application.Features.Auth.Services;

namespace UniversitySystem.Application.Features.Auth.Commands.Login;

/// <summary>
/// درخواست «ورود کاربر و دریافت توکن» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return authService.LoginAsync(request.Username, request.Password, cancellationToken);
    }
}
