using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.MajorCurricula;

/// <summary>قرارداد داده رشته و چارت؛ تراکنش ساخت درس و ارتباط آن با چارت در همین مرز قرار دارد.</summary>
public interface IMajorCurriculumRepository
{
    Task<ICollection<MajorOptionDto>> GetMajorsAsync(CancellationToken ct);
    Task<Major?> GetMajorAsync(long majorId, CancellationToken ct);
    Task<Curriculum?> GetCurrentAsync(long majorId, CancellationToken ct);
    Task<ICollection<Course>> GetCoursesAsync(ICollection<long> ids, CancellationToken ct);
    Task<string> GetNextCourseCodeAsync(CancellationToken ct);
    Task<bool> HasCurrentStudentUseAsync(long majorId, ICollection<long> courseIds, CancellationToken ct);
    void Add(Curriculum curriculum);
    void Add(Course course);
    Task<T> ExecuteSerializableAsync<T>(Func<Task<T>> action, CancellationToken ct);
}
