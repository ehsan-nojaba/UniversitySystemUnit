using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Repositories;

/// <summary>
/// داده موردنیاز تداخل‌سنجی یک استاد: نام، برنامه ارائه‌های دیگر همان ترم و زمان‌های آزاد اعلام‌شده.
/// </summary>
public sealed record ProfessorScheduleData(long ProfessorId, string FullName,
    ICollection<CourseOfferingScheduleSlotDto> OtherSchedules,
    ICollection<CourseOfferingScheduleSlotDto> Availability);
