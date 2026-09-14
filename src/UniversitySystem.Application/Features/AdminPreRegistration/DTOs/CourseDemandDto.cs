namespace UniversitySystem.Application.Features.AdminPreRegistration.DTOs;

/// <summary>
/// Aggregated student demand metrics for a course within a specific academic term.
/// </summary>
public class CourseDemandDto
{
    public long CourseId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public double AveragePriority { get; set; }
    public int TotalRequestedCredits { get; set; }
}
