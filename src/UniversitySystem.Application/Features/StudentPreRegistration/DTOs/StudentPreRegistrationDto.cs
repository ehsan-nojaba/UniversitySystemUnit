namespace UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

/// <summary>
/// پاسخ پیش‌انتخاب دانشجو: شناسه درخواست، ترم، وضعیت، درس‌های اولویت‌بندی‌شده، مجموع واحد و زمان ارسال.
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
