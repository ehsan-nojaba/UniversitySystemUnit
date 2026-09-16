namespace UniversitySystem.Application.Features.AdminPlanning.DTOs;

/// <summary>
/// نمای برنامه‌ریزی یک ترم؛ مشخصات ترم و درس‌های دارای تقاضای دانشجو یا علاقه استاد را جمع می‌کند.
/// </summary>
public class AcademicPlanningOverviewDto
{
    public long AcademicTermId { get; set; }
    public string AcademicTermCode { get; set; } = string.Empty;
    public string AcademicTermTitle { get; set; } = string.Empty;
    public ICollection<CoursePlanningOverviewDto> Courses { get; set; } = [];
}
