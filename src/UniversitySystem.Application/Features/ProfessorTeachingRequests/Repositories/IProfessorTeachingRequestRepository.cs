using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Repositories;

/// <summary>
/// قرارداد دسترسی به داده بخش «درخواست تدریس و زمان‌های آزاد استاد»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند.
/// </summary>
public interface IProfessorTeachingRequestRepository
{
    Task<Professor?> GetProfessorAsync(long userId, CancellationToken cancellationToken);
    Task<AcademicTerm?> GetTermAsync(long termId, CancellationToken cancellationToken);
    Task<ProfessorTeachingRequest?> GetAsync(long professorId, long termId, CancellationToken cancellationToken);
    Task<ICollection<Course>> GetCoursesAsync(ICollection<long> ids, CancellationToken cancellationToken);
    Task<ICollection<ProfessorTeachingRequest>> GetSubmittedAsync(long termId, CancellationToken cancellationToken);
    void Add(ProfessorTeachingRequest request);
}
