using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Services;
using UniversitySystem.Application.Features.AdminReports.DTOs;
using UniversitySystem.Application.Features.AdminReports.Repositories;

namespace UniversitySystem.Application.Features.AdminReports.Services;

/// <summary>
/// داده گزارش ارائه‌ها را با نمای تقاضای آموزش ترکیب می‌کند و درس‌های دارای تقاضا بدون ارائه فعال و مجموع ظرفیت و ثبت‌نام را برمی‌گرداند؛ داده را تغییر نمی‌دهد.
/// </summary>
public sealed class AdminReportService(IAdminReportRepository repository, IAdminPlanningService planning)
{
    public async Task<AdminReportDto> GetAsync(long termId, CancellationToken cancellationToken)
    {
        var overview = await planning.GetPlanningOverviewAsync(termId, cancellationToken);
        var offerings = await repository.GetOfferingsAsync(termId, cancellationToken);
        var offeredIds = offerings.Where(o => o.IsActive).Select(o => o.CourseId).ToHashSet();
        return new(termId, offerings.Where(o => o.IsActive).Sum(o => o.Capacity), offerings.Sum(o => o.EnrolledCount), offerings,
            overview.Courses.Where(c => c.StudentDemandCount > 0 && !offeredIds.Contains(c.CourseId)).ToList());
    }
}
