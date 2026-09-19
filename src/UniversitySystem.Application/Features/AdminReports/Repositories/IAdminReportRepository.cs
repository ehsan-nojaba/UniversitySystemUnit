using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Services;
using UniversitySystem.Application.Features.AdminReports.DTOs;

namespace UniversitySystem.Application.Features.AdminReports.Repositories;

/// <summary>
/// قرارداد دسترسی به داده بخش «گزارش ظرفیت و وضعیت ارائه‌های آموزش»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند.
/// </summary>
public interface IAdminReportRepository
{
    Task<ICollection<OfferingReportDto>> GetOfferingsAsync(long termId, CancellationToken cancellationToken);
}
