using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// قرارداد محاسبه درس‌های مجاز دانشجو با توجه به چارت، درس‌های پاس‌شده و پیش‌نیازها؛ در پیش‌انتخاب و ثبت‌نام دوباره استفاده می‌شود.
/// </summary>
public interface IStudentCourseEligibilityService
{
    /// <summary>
    /// Computes the list of eligible courses for a student for the given academic term.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="academicTermId">The target academic term identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of eligible courses with prerequisite details.</returns>
    Task<ICollection<EligibleCourseDto>> GetEligibleCoursesAsync(long studentId, long academicTermId, CancellationToken cancellationToken = default);
}
