namespace UniversitySystem.Application.Features.AdminPreRegistration.DTOs;

/// <summary>
/// Aggregated student demand metrics for a course within a specific academic term.
/// </summary>
public sealed record CourseDemandDto(
    long CourseId,
    string CourseCode,
    string CourseTitle,
    int StudentCount,
    double AveragePriority,
    int TotalRequestedCredits
);
