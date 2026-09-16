namespace UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

/// <summary>
/// مشخصات یک درس انتخاب‌شده در پاسخ پیش‌انتخاب: شناسه، کد، عنوان، واحد و اولویت دانشجو.
/// </summary>
public class PreRegistrationCourseItemDto
{
    public long CourseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int Priority { get; set; }
}
