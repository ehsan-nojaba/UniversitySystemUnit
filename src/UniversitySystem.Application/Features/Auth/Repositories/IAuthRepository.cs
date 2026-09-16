using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.Auth.Repositories;

/// <summary>
/// قرارداد دسترسی به داده بخش «ورود و احراز هویت»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند.
/// </summary>
public interface IAuthRepository
{
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<string[]> GetUserRolesAsync(long userId, CancellationToken cancellationToken = default);
}
