using UniversitySystem.Application.Features.Auth.Commands.Login;

namespace UniversitySystem.Application.Features.Auth.Services;

/// <summary>
/// قرارداد عملیات بخش «ورود و احراز هویت»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد.
/// </summary>
public interface IAuthService
{
    Task<LoginResponse> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
}
