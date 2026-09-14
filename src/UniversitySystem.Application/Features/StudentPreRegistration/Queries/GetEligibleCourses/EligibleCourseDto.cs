namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

/// <summary>
/// Represents a course that a student is eligible to select for pre-registration.
/// </summary>
public class EligibleCourseDto
{
    public long CourseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int RecommendedTerm { get; set; }
    public bool IsRequired { get; set; }
    public ICollection<CoursePrerequisiteDto> Prerequisites { get; set; } = [];
}

/// <summary>
/// Brief summary of a prerequisite course.
/// </summary>
public class CoursePrerequisiteDto
{
    public long PrerequisiteCourseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}
