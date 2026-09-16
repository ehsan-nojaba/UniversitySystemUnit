namespace UniversitySystem.Application.Features.AdminPreRegistration.DTOs;

/// <summary>
/// خلاصه تقاضای یک درس از پیش‌انتخاب‌های ارسال‌شده دانشجویان؛ برای تصمیم ارائه استفاده می‌شود.
/// </summary>
public class CourseDemandDto
{
    public long CourseId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public double AveragePriority { get; set; }
    public int TotalRequestedCredits { get; set; }
}
