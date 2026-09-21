using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Application.Features.CourseOfferings.Services;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.Application.Features.TeachingAssignments.Services;
/// <summary>
/// اجرای قواعد و هماهنگی عملیات بخش «تخصیص استاد به ارائه درس»؛ داده را از ریپازیتوری می‌گیرد و تغییرات را از طریق مدل‌های دامنه انجام می‌دهد.
/// </summary>
public sealed class TeachingAssignmentService(ITeachingAssignmentRepository repository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, ICourseOfferingRepository offeringRepository, IProfessorScheduleConflictChecker conflictChecker, UniversitySystem.Application.Features.AcademicWorkflow.IAcademicWorkflowRepository workflow) : ITeachingAssignmentService
{
    public async Task<ICollection<TeachingAssignmentDto>> GetAssignmentsAsync(long courseOfferingId, CancellationToken cancellationToken = default)
    {
        if (courseOfferingId <= 0)
        {
            throw new BusinessException("شناسه ارائه درس نامعتبر است.");
        }

        var offering = await repository.GetCourseOfferingAsync(courseOfferingId, cancellationToken);
        if (offering is null)
        {
            throw new NotFoundException(nameof(CourseOffering), courseOfferingId);
        }

        return await repository.GetAssignmentsByOfferingIdAsync(courseOfferingId, cancellationToken);
    }

    public async Task<TeachingAssignmentDto> AssignProfessorAsync(long courseOfferingId, long professorId, CancellationToken cancellationToken = default)
    {
        if (courseOfferingId <= 0)
        {
            throw new BusinessException("شناسه ارائه درس نامعتبر است.");
        }

        if (professorId <= 0)
        {
            throw new BusinessException("شناسه استاد نامعتبر است.");
        }

        var offering = await repository.GetCourseOfferingAsync(courseOfferingId, cancellationToken);
        if (offering is null)
        {
            throw new NotFoundException(nameof(CourseOffering), courseOfferingId);
        }

        if (offering.IsFinalized) { throw new BusinessException("ابتدا ارائه را به برنامه‌ریزی برگردانید."); }
        var allowed = await workflow.GetAllowedCourseIdsAsync(professorId, cancellationToken);
        if (!allowed.Contains(offering.CourseId) || !await workflow.HasSubmittedProposalAsync(professorId, offering.CourseId, offering.AcademicTermId, cancellationToken)) { throw new BusinessException("استاد باید برای این درس مجاز باشد و پیشنهاد زمانی همین درس را ارسال کرده باشد."); }
        var professor = await repository.GetProfessorAsync(professorId, cancellationToken);
        if (professor is null)
        {
            throw new NotFoundException(nameof(Professor), professorId);
        }

        var isAssigned = await repository.IsProfessorAssignedAsync(courseOfferingId, professorId, cancellationToken);
        if (isAssigned)
        {
            throw new BusinessException("این استاد قبلاً به این ارائه تخصیص داده شده است.");
        }

        var term = await offeringRepository.GetAcademicTermAsync(offering.AcademicTermId, cancellationToken);
        if (!offering.IsActive || term is null || !term.IsActive)
        {
            throw new BusinessException("Offering and academic term must be active.");
        }

        var schedules = await offeringRepository.GetSchedulesByOfferingIdAsync(courseOfferingId, cancellationToken);
        await conflictChecker.CheckProfessorAssignmentAsync(courseOfferingId, professorId, schedules.Select(s => new CourseOfferingScheduleSlotDto { DayOfWeek = s.DayOfWeek, StartTime = s.StartTime, EndTime = s.EndTime }).ToList(), cancellationToken);
        var assignedAt = dateTimeProvider.UtcNow;
        var assignment = TeachingAssignmentLogic.Create(courseOfferingId,professorId,assignedAt);
        repository.Add(assignment);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var(requested, priority) = await repository.GetProfessorCourseRequestInfoAsync(professorId, offering.AcademicTermId, offering.CourseId, cancellationToken);
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
        if (courseOfferingId <= 0)
        {
            throw new BusinessException("شناسه ارائه درس نامعتبر است.");
        }

        if (professorId <= 0)
        {
            throw new BusinessException("شناسه استاد نامعتبر است.");
        }

        var offering = await repository.GetCourseOfferingAsync(courseOfferingId, cancellationToken);
        if (offering is null)
        {
            throw new NotFoundException(nameof(CourseOffering), courseOfferingId);
        }

        if (offering.IsFinalized) { throw new BusinessException("ابتدا ارائه را به برنامه‌ریزی برگردانید."); }
        var assignment = await repository.GetAssignmentAsync(courseOfferingId, professorId, cancellationToken);
        if (assignment is null)
        {
            throw new NotFoundException("تخصیص تدریس مورد نظر یافت نشد.");
        }

        repository.Remove(assignment);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

