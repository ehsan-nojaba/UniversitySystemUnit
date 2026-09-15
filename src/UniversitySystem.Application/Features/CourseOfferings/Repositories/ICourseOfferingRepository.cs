using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.CourseOfferings.Repositories;

public interface ICourseOfferingRepository
{
    Task<AcademicTerm?> GetAcademicTermAsync(long termId, CancellationToken cancellationToken = default);
    Task<Course?> GetCourseAsync(long courseId, CancellationToken cancellationToken = default);
    Task<CourseOffering?> GetByIdWithCourseAsync(long id, CancellationToken cancellationToken = default);
    Task<CourseOffering?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> OfferingExistsAsync(long termId, long courseId, CancellationToken cancellationToken = default);
    Task<ICollection<CourseOfferingDto>> GetOfferingsByTermAsync(long termId, CancellationToken cancellationToken = default);
    Task<ICollection<CourseOfferingScheduleDto>> GetSchedulesByOfferingIdAsync(long offeringId, CancellationToken cancellationToken = default);
    Task SaveSchedulesAsync(long offeringId, IReadOnlyCollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default);
    void Add(CourseOffering offering);
}
