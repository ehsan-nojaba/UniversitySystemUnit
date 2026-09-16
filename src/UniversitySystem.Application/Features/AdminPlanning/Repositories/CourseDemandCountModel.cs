namespace UniversitySystem.Application.Features.AdminPlanning.Repositories;

/// <summary>
/// خروجی تجمیع تقاضا شامل شناسه درس و تعداد درخواست‌های ارسال‌شده دانشجو.
/// </summary>
public class CourseDemandCountModel
{
    public long CourseId { get; set; }
    public int DemandCount { get; set; }
}
