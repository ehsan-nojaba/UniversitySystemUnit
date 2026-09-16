using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.AdminPreRegistration.Services;

/// <summary>
/// قرارداد عملیات بخش «جمع‌بندی تقاضای درس دانشجویان»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد.
/// </summary>
public interface IAdminPreRegistrationService
{
    Task<ICollection<CourseDemandDto>> GetCourseDemandSummaryAsync(long academicTermId, CancellationToken cancellationToken = default);
}
