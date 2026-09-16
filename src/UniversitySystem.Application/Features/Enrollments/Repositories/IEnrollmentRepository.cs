using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.Enrollments.Repositories;
/// <summary>
/// قرارداد دسترسی به داده بخش «ثبت‌نام قطعی دانشجو»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند.
/// </summary>
public interface IEnrollmentRepository
{
    Task<T> ExecuteSerializableAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken);
    Task<CourseOffering?> GetOfferingAsync(long id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Enrollment>> GetStudentEnrollmentsAsync(long studentId, long termId, CancellationToken cancellationToken);
    Task<int> GetEnrollmentCountAsync(long offeringId, CancellationToken cancellationToken);
    void Add(Enrollment enrollment);
}
