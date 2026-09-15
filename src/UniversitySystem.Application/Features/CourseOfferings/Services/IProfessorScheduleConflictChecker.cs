using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Services;

/// <summary>
/// سرویس بررسی تداخل زمانی و تطابق بازه حضور اساتید در زمان‌بندی ارائه دروس.
/// </summary>
public interface IProfessorScheduleConflictChecker
{
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
