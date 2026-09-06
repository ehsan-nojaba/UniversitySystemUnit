namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// Provides the current UTC date and time.
///
/// Abstracting <see cref="DateTime.UtcNow"/> behind an interface makes
/// time-dependent code deterministic in tests and prevents accidental use
/// of local time in a distributed, multi-timezone system.
///
/// Implementation: <c>UniversitySystem.Infrastructure.Services.DateTimeProvider</c>
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>Returns the current date and time in UTC.</summary>
    DateTime UtcNow { get; }
}
