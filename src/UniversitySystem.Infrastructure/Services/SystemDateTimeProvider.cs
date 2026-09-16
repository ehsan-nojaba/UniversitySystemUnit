using UniversitySystem.Application.Common.Interfaces;

namespace UniversitySystem.Infrastructure.Services;

/// <summary>
/// زمان محلی و UTC سیستم را ارائه می‌دهد؛ ذخیره زمان‌های آموزشی و حسابرسی از UTC استفاده می‌کند.
/// </summary>
public class SystemDateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTime Now => DateTime.Now;

    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}
