namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

/// <summary>
/// Represents a course that a student is eligible to select for pre-registration.
/// </summary>
public sealed record EligibleCourseDto(
    long CourseId,
    string Code,
    string Title,
    int Credits,
    int RecommendedTerm,
    bool IsRequired,
    IReadOnlyList<CoursePrerequisiteDto> Prerequisites
);

/// <summary>
/// Brief summary of a prerequisite course.
/// </summary>
public sealed record CoursePrerequisiteDto(
    long PrerequisiteCourseId,
    string Code,
    string Title
);
