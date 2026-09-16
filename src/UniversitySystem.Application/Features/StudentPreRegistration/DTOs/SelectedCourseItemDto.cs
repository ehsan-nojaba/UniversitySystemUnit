namespace UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

/// <summary>
/// ورودی انتخاب درس دانشجو؛ فقط شناسه درس و اولویت را دریافت می‌کند و مالک درخواست از کاربر جاری تعیین می‌شود.
/// </summary>
public class SelectedCourseItemDto
{
    public long CourseId { get; set; }
    public int Priority { get; set; }
}
