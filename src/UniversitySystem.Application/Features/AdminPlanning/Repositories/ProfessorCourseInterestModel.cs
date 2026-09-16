namespace UniversitySystem.Application.Features.AdminPlanning.Repositories;

/// <summary>
/// خروجی علاقه استاد به درس شامل شناسه‌ها، نام استاد و اولویت؛ برای جمع‌بندی آموزش استفاده می‌شود.
/// </summary>
public class ProfessorCourseInterestModel
{
    public long CourseId { get; set; }
    public long ProfessorId { get; set; }
    public string ProfessorFullName { get; set; } = string.Empty;
    public int Priority { get; set; }
}
