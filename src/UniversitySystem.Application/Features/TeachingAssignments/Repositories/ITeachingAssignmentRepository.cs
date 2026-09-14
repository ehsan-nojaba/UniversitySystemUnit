using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.TeachingAssignments.Repositories;

public interface ITeachingAssignmentRepository
{
    Task<CourseOffering?> GetCourseOfferingAsync(long offeringId, CancellationToken cancellationToken = default);
    Task<Professor?> GetProfessorAsync(long professorId, CancellationToken cancellationToken = default);
    Task<string> GetProfessorFullNameAsync(long professorId, CancellationToken cancellationToken = default);
    Task<bool> IsProfessorAssignedAsync(long offeringId, long professorId, CancellationToken cancellationToken = default);
    Task<TeachingAssignment?> GetAssignmentAsync(long offeringId, long professorId, CancellationToken cancellationToken = default);
    Task<(bool Requested, int? Priority)> GetProfessorCourseRequestInfoAsync(long professorId, long termId, long courseId, CancellationToken cancellationToken = default);
    Task<ICollection<TeachingAssignmentDto>> GetAssignmentsByOfferingIdAsync(long offeringId, CancellationToken cancellationToken = default);
    void Add(TeachingAssignment assignment);
    void Remove(TeachingAssignment assignment);
}
