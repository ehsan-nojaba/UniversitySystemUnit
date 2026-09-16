namespace UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

/// <summary>
/// مشخصات نمایشی یک پیش‌نیاز: شناسه، کد و عنوان درس پیش‌نیاز؛ رابطه اصلی در CoursePrerequisite ذخیره می‌شود.
/// </summary>
public class CoursePrerequisiteDto
{
    public long PrerequisiteCourseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}
