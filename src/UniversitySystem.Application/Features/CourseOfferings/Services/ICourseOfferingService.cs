using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Services;

public interface ICourseOfferingService
{
    Task<ICollection<CourseOfferingDto>> GetOfferingsByTermAsync(long academicTermId, CancellationToken cancellationToken = default);
    Task<CourseOfferingDto> CreateOfferingAsync(long academicTermId, long courseId, int capacity, CancellationToken cancellationToken = default);
    Task<CourseOfferingDto> UpdateOfferingAsync(long id, int capacity, bool? isActive, CancellationToken cancellationToken = default);
}
