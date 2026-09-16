using MediatR;

namespace UniversitySystem.Application.Features.Auth.Commands.Login;

/// <summary>
/// درخواست انجام عملیات «ورود کاربر و دریافت توکن»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public class LoginCommand : IRequest<LoginResponse>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
