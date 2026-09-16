namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// قرارداد دریافت زمان؛ امکان استفاده از زمان کنترل‌شده در تست را فراهم می‌کند.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>Returns the current date and time.</summary>
    DateTime Now { get; }

    /// <summary>Returns the current UTC date and time.</summary>
    DateTime UtcNow { get; }
}
