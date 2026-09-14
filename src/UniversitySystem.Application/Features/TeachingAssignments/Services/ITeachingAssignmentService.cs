using UniversitySystem.Application.Features.TeachingAssignments.DTOs;

namespace UniversitySystem.Application.Features.TeachingAssignments.Services;

public interface ITeachingAssignmentService
{
    Task<ICollection<TeachingAssignmentDto>> GetAssignmentsAsync(long courseOfferingId, CancellationToken cancellationToken = default);
    Task<TeachingAssignmentDto> AssignProfessorAsync(long courseOfferingId, long professorId, CancellationToken cancellationToken = default);
    Task RemoveAssignmentAsync(long courseOfferingId, long professorId, CancellationToken cancellationToken = default);
}
