namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// Provides the current date and time.
///
/// Abstracting <see cref="DateTime.Now"/> behind an interface makes
/// time-dependent code deterministic and testable.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>Returns the current date and time.</summary>
    DateTime Now { get; }
}
