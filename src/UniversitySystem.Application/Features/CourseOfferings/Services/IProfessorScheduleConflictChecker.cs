using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Services;

/// <summary>
/// مدل کمکی IProfessorScheduleConflictChecker؛ مسئولیت آن در راهنمای فارسی پروژه توضیح داده شده است.
/// </summary>
public interface IProfessorScheduleConflictChecker
{
    Task CheckProfessorAssignmentAsync(long courseOfferingId, long professorId, IReadOnlyCollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default);
    /// <summary>
    /// بررسی تداخل زمانی اساتید تخصیص‌یافته به یک ارائه درس با سایر ارائه‌های همان ترم
    /// و اعتبارسنجی انطباق با بازه‌های زمانی اعلام‌شده حضور استاد در آن ترم.
    /// </summary>
    /// <param name="courseOfferingId">شناسه ارائه درس</param>
    /// <param name="slots">اسلات‌های زمانی پیشنهادی</param>
    /// <param name="cancellationToken">توکن لغو عملیات</param>
    Task CheckConflictsAsync(
        long courseOfferingId,
        IReadOnlyCollection<CourseOfferingScheduleSlotDto> slots,
        CancellationToken cancellationToken = default);
}
