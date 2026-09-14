using UniversitySystem.Application.Common.Interfaces;

namespace UniversitySystem.Infrastructure.Services;

/// <summary>
/// Provides current system date and time using <see cref="DateTime.Now"/>.
/// </summary>
public class SystemDateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTime Now => DateTime.Now;

    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}
