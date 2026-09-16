using StudentPreRegistrationEntity = UniversitySystem.Domain.Entities.StudentPreRegistration;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Repositories;

/// <summary>
/// قرارداد دسترسی به داده بخش «پیش‌انتخاب واحد دانشجو»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند.
/// </summary>
public interface IStudentPreRegistrationRepository
{
    Task<Student?> GetStudentByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<AcademicTerm?> GetAcademicTermAsync(long termId, CancellationToken cancellationToken = default);
    Task<StudentPreRegistrationEntity?> GetPreRegistrationWithItemsAsync(long studentId, long termId, CancellationToken cancellationToken = default);
    Task<StudentPreRegistrationEntity?> GetPreRegistrationWithItemsAndCoursesAsync(long studentId, long termId, CancellationToken cancellationToken = default);
    void AddPreRegistration(StudentPreRegistrationEntity preRegistration);
}
