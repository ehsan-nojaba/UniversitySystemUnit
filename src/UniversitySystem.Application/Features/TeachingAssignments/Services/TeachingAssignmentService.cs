using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Repositories;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.TeachingAssignments.Services;

public sealed class TeachingAssignmentService(
    ITeachingAssignmentRepository repository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : ITeachingAssignmentService
{
    public async Task<ICollection<TeachingAssignmentDto>> GetAssignmentsAsync(long courseOfferingId, CancellationToken cancellationToken = default)
    {
        var offering = await repository.GetCourseOfferingAsync(courseOfferingId, cancellationToken);
        if (offering is null) throw new NotFoundException(nameof(CourseOffering), courseOfferingId);

        return await repository.GetAssignmentsByOfferingIdAsync(courseOfferingId, cancellationToken);
    }

    public async Task<TeachingAssignmentDto> AssignProfessorAsync(long courseOfferingId, long professorId, CancellationToken cancellationToken = default)
    {
        var offering = await repository.GetCourseOfferingAsync(courseOfferingId, cancellationToken);
        if (offering is null) throw new NotFoundException(nameof(CourseOffering), courseOfferingId);

        var professor = await repository.GetProfessorAsync(professorId, cancellationToken);
        if (professor is null) throw new NotFoundException(nameof(Professor), professorId);

        var isAssigned = await repository.IsProfessorAssignedAsync(courseOfferingId, professorId, cancellationToken);
        if (isAssigned) throw new BusinessException("این استاد قبلاً به این ارائه تخصیص داده شده است.");

        var assignedAt = dateTimeProvider.UtcNow;
        var assignment = new TeachingAssignment(courseOfferingId, professorId, assignedAt);
        repository.Add(assignment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var (requested, priority) = await repository.GetProfessorCourseRequestInfoAsync(professorId, offering.AcademicTermId, offering.CourseId, cancellationToken);
        var fullName = await repository.GetProfessorFullNameAsync(professorId, cancellationToken);

        return new TeachingAssignmentDto
        {
            TeachingAssignmentId = assignment.Id,
            ProfessorId = professorId,
            ProfessorFullName = fullName,
            AssignedAt = assignedAt,
            RequestedThisCourse = requested,
            RequestedPriority = priority
        };
    }

    public async Task RemoveAssignmentAsync(long courseOfferingId, long professorId, CancellationToken cancellationToken = default)
    {
        var offering = await repository.GetCourseOfferingAsync(courseOfferingId, cancellationToken);
        if (offering is null) throw new NotFoundException(nameof(CourseOffering), courseOfferingId);

        var assignment = await repository.GetAssignmentAsync(courseOfferingId, professorId, cancellationToken);
        if (assignment is null) throw new NotFoundException("تخصیص تدریس مورد نظر یافت نشد.");

        repository.Remove(assignment);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
