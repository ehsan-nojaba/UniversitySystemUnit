namespace UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

/// <summary>
/// خروجی یک درس مجاز برای پیش‌انتخاب: کد، عنوان، تعداد واحد، ترم پیشنهادی، الزامی بودن و مشخصات پیش‌نیازها.
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
