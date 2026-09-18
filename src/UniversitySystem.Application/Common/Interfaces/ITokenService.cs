using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// قرارداد تولید توکن ورود و زمان انقضا؛ تنظیمات امضای JWT در Infrastructure قرار دارند.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// برای کاربر احرازشده، توکن امضاشده شامل هویت و نقش‌ها و زمان انقضای UTC تولید می‌کند.
    /// </summary>
    /// <param name="user">کاربری که رمز و وضعیت فعال بودن او بررسی شده است.</param>
    /// <param name="roles">نقش‌های خوانده‌شده از حساب کاربر.</param>
    /// <returns>رشته توکن و زمان پایان اعتبار آن در UTC.</returns>
    (string AccessToken, DateTime ExpiresAt) GenerateToken(User user, IEnumerable<string> roles);
}
