namespace UniversitySystem.Application.Features.AdminPlanning.Repositories;

/// <summary>
/// خروجی سبک مشخصات درس برای گزارش برنامه‌ریزی؛ شامل شناسه، کد، عنوان و واحد است.
/// </summary>
public class CourseInfoModel
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
}
