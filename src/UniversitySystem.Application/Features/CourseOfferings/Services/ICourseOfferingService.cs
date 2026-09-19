using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Services;

/// <summary>
/// قرارداد عملیات بخش «ارائه درس و برنامه زمانی کلاس»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد.
/// </summary>
public interface ICourseOfferingService
{
    Task<ICollection<CourseOfferingDto>> GetOfferingsByTermAsync(long academicTermId, CancellationToken cancellationToken = default);
    Task<CourseOfferingDto> CreateOfferingAsync(long academicTermId, long courseId, int capacity, CancellationToken cancellationToken = default);
    Task<CourseOfferingDto> UpdateOfferingAsync(long id, int capacity, bool? isActive, CancellationToken cancellationToken = default);
    Task<ICollection<CourseOfferingScheduleDto>> GetScheduleAsync(long courseOfferingId, CancellationToken cancellationToken = default);
    Task<ICollection<CourseOfferingScheduleDto>> SaveScheduleAsync(long courseOfferingId, ICollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default);
}
