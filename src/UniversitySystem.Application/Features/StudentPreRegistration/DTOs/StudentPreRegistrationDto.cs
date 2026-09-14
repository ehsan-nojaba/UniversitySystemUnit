namespace UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

/// <summary>
/// Summary representation of a student's pre-registration draft or submission.
/// </summary>
public class StudentPreRegistrationDto
{
    public long PreRegistrationId { get; set; }
    public long AcademicTermId { get; set; }
    public string Status { get; set; } = string.Empty;
    public ICollection<PreRegistrationCourseItemDto> Courses { get; set; } = [];
    public int TotalCredits { get; set; }
    public DateTime? SubmittedAt { get; set; }
}

/// <summary>
/// Course item in pre-registration response.
/// </summary>
public class PreRegistrationCourseItemDto
{
    public long CourseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int Priority { get; set; }
}

/// <summary>
/// Course item submitted in a save request.
/// </summary>
public class SelectedCourseItemDto
{
    public long CourseId { get; set; }
    public int Priority { get; set; }
}
