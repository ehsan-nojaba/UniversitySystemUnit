using UniversitySystem.Application.Features.MajorCourses.DTOs;

namespace UniversitySystem.Application.Features.MajorCourses.Services;

/// <summary>
/// قرارداد عملیات بخش «مدیریت دروس رشته»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد.
/// </summary>
public interface IMajorCourseService
{
    Task<ICollection<MajorOptionDto>> GetMajorsAsync(CancellationToken cancellationToken = default);
    Task<ICollection<MajorCourseDto>> GetCoursesForMajorAsync(long majorId, CancellationToken cancellationToken = default);
    Task<MajorCourseDto> CreateCourseForMajorAsync(long majorId, string code, string title, int credits, int recommendedTerm, bool isRequired, CancellationToken cancellationToken = default);
    Task<MajorCourseDto> AddExistingCourseToMajorAsync(long majorId, long courseId, int recommendedTerm, bool isRequired, CancellationToken cancellationToken = default);
    Task RemoveCourseFromMajorAsync(long majorId, long courseId, CancellationToken cancellationToken = default);
}
