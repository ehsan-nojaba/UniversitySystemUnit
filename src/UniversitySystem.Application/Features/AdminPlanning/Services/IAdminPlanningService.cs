using UniversitySystem.Application.Features.AdminPlanning.DTOs;

namespace UniversitySystem.Application.Features.AdminPlanning.Services;

/// <summary>
/// قرارداد عملیات بخش «برنامه‌ریزی آموزش بر اساس تقاضا و علاقه استاد»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد.
/// </summary>
public interface IAdminPlanningService
{
    Task<AcademicPlanningOverviewDto> GetPlanningOverviewAsync(long academicTermId, CancellationToken cancellationToken = default);
}
