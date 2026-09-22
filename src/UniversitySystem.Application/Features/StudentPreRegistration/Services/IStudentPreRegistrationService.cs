using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Services;

/// <summary>
/// قرارداد عملیات بخش «پیش‌انتخاب واحد دانشجو»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد.
/// </summary>
public interface IStudentPreRegistrationService
{
    Task<StudentPreRegistrationDto> SaveDraftAsync(long academicTermId, ICollection<SelectedCourseItemDto> courses, CancellationToken cancellationToken = default);
    Task<StudentPreRegistrationDto> SubmitAsync(long academicTermId, CancellationToken cancellationToken = default);
    Task<StudentPreRegistrationDto> StartNewAttemptAsync(long academicTermId, CancellationToken cancellationToken = default);
    Task<StudentPreRegistrationDto?> GetByTermAsync(long academicTermId, CancellationToken cancellationToken = default);
    Task<ICollection<EligibleCourseDto>> GetEligibleCoursesAsync(long academicTermId, CancellationToken cancellationToken = default);
}
