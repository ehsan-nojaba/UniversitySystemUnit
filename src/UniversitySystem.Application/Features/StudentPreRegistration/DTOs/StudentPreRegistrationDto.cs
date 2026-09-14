namespace UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

/// <summary>
/// Summary representation of a student's pre-registration draft or submission.
/// </summary>
public sealed record StudentPreRegistrationDto(
    long PreRegistrationId,
    long AcademicTermId,
    string Status,
    IReadOnlyList<PreRegistrationCourseItemDto> Courses,
    int TotalCredits,
    DateTime? SubmittedAt = null
);

/// <summary>
/// Course item in pre-registration response.
/// </summary>
public sealed record PreRegistrationCourseItemDto(
    long CourseId,
    string Code,
    string Title,
    int Credits,
    int Priority
);

/// <summary>
/// Course item submitted in a save request.
/// </summary>
public sealed record SelectedCourseItemDto(
    long CourseId,
    int Priority
);
