using UniversitySystem.Application.Features.TeachingAssignments.DTOs;

namespace UniversitySystem.Application.Features.TeachingAssignments.Services;

/// <summary>
/// قرارداد عملیات بخش «تخصیص استاد به ارائه درس»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد.
/// </summary>
public interface ITeachingAssignmentService
{
    Task<ICollection<TeachingAssignmentDto>> GetAssignmentsAsync(long courseOfferingId, CancellationToken cancellationToken = default);
    Task<TeachingAssignmentDto> AssignProfessorAsync(long courseOfferingId, long professorId, CancellationToken cancellationToken = default);
    Task RemoveAssignmentAsync(long courseOfferingId, long professorId, CancellationToken cancellationToken = default);
}
