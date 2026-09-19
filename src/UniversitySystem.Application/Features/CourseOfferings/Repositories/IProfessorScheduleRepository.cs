using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Repositories;

/// <summary>
/// قرارداد دسترسی به داده بخش «ارائه درس و برنامه زمانی کلاس»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند.
/// </summary>
public interface IProfessorScheduleRepository
{
    Task<ICollection<ProfessorScheduleData>> GetDataAsync(long offeringId, long? additionalProfessorId, CancellationToken cancellationToken);
}
