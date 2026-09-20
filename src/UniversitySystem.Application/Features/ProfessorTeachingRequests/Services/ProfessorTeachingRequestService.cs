using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Services;
/// <summary>
/// درخواست استاد جاری را مدیریت می‌کند: ذخیره درس‌ها، ویرایش زمان آزاد، ارسال نهایی و تهیه خلاصه درخواست‌های ارسال‌شده برای آموزش.
/// </summary>
public sealed class ProfessorTeachingRequestService(IProfessorTeachingRequestRepository repository, ICurrentUserService currentUser, IUnitOfWork unitOfWork, IDateTimeProvider clock, UniversitySystem.Application.Features.AcademicWorkflow.IAcademicWorkflowRepository workflow)
{
    private async Task<long> GetProfessorIdAsync(CancellationToken cancellationToken)
    {
        if (!long.TryParse(currentUser.UserId, out var id))
        {
            throw new UnauthorizedAccessException();
        }

        return (await repository.GetProfessorAsync(id, cancellationToken) ?? throw new NotFoundException("Professor profile was not found.")).Id;
    }

    private async Task EnsureTermAsync(long termId, bool active, CancellationToken cancellationToken)
    {
        var term = await repository.GetTermAsync(termId, cancellationToken) ?? throw new NotFoundException(nameof(AcademicTerm), termId);
        if (active && !term.IsActive)
        {
            throw new BusinessException("Academic term must be active.");
        }
    }

    private async Task<ProfessorTeachingRequest> GetDraftAsync(long termId, CancellationToken cancellationToken)
    {
        var id = await GetProfessorIdAsync(cancellationToken);
        await EnsureTermAsync(termId, true, cancellationToken);
        var request = await repository.GetAsync(id, termId, cancellationToken) ?? throw new NotFoundException("Teaching request was not found.");
        if (request.Status != RequestStatus.Draft)
        {
            throw new BusinessException("Only draft teaching requests can be changed.");
        }

        return request;
    }

    public async Task<TeachingRequestDto?> GetAsync(long termId, CancellationToken cancellationToken)
    {
        var id = await GetProfessorIdAsync(cancellationToken);
        await EnsureTermAsync(termId, false, cancellationToken);
        var request = await repository.GetAsync(id, termId, cancellationToken);
        return request is null ? null : Map(request);
    }

    public async Task<TeachingRequestDto> SaveAsync(long termId, ICollection<TeachingCourseInput> courses, CancellationToken cancellationToken)
    {
        var id = await GetProfessorIdAsync(cancellationToken);
        await EnsureTermAsync(termId, true, cancellationToken);
        var allowed = await workflow.GetAllowedCourseIdsAsync(id, cancellationToken);
        if (courses.Any(c => !allowed.Contains(c.CourseId))) { throw new BusinessException("فقط درس‌های مجاز تعیین‌شده توسط آموزش قابل انتخاب‌اند."); }
        var ids = courses.Select(c => c.CourseId).ToHashSet();
        var available = await repository.GetCoursesAsync(ids, cancellationToken);
        if (available.Count != ids.Count || available.Any(c => !c.IsActive))
        {
            throw new BusinessException("All selected courses must exist and be active.");
        }

        var request = await repository.GetAsync(id, termId, cancellationToken);
        if (request is null)
        {
            request = ProfessorTeachingRequestLogic.Create(id,termId);
            repository.Add(request);
        }

        if (request.Status != RequestStatus.Draft)
        {
            throw new BusinessException("Only drafts can be changed.");
        }

        foreach (var c in request.Courses.Where(c => !ids.Contains(c.CourseId)).ToList())
            ProfessorTeachingRequestLogic.RemoveCourse(            request,c.CourseId);
        foreach (var c in courses)
            if (request.Courses.Any(e => e.CourseId == c.CourseId))
            {
                ProfessorTeachingRequestLogic.UpdateCoursePriority(                request,c.CourseId,c.Priority);
            }
            else
            {
                ProfessorTeachingRequestLogic.AddCourse(                request,c.CourseId,c.Priority);
            }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(request, available);
    }

    public async Task<TeachingRequestDto> SaveAvailabilityAsync(long termId, ICollection<AvailabilityInput> availability, CancellationToken cancellationToken)
    {
        var request = await GetDraftAsync(termId, cancellationToken);
        if (availability.Any(a => !a.CourseId.HasValue || !request.Courses.Any(c => c.CourseId == a.CourseId))) { throw new BusinessException("برای هر زمان پیشنهادی، یکی از درس‌های انتخاب‌شده را مشخص کنید."); }
        ProfessorTeachingRequestLogic.ReplaceCourseAvailability(        request,availability.Select(a => (a.CourseId!.Value, a.DayOfWeek, a.StartTime, a.EndTime)));
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(request);
    }

    public async Task<TeachingRequestDto> SubmitAsync(long termId, CancellationToken cancellationToken)
    {
        var request = await GetDraftAsync(termId, cancellationToken);
        if (request.Courses.Count == 0)
        {
            throw new BusinessException("At least one course is required.");
        }

        var courses = await repository.GetCoursesAsync(request.Courses.Select(c => c.CourseId).ToList(), cancellationToken);
        if (courses.Any(c => !c.IsActive))
        {
            throw new BusinessException("Selected courses must be active.");
        }

        var allowed = await workflow.GetAllowedCourseIdsAsync(request.ProfessorId, cancellationToken);
        if (request.Courses.Any(c => !allowed.Contains(c.CourseId) || !request.Availabilities.Any(a => a.CourseId == c.CourseId))) { throw new BusinessException("برای هر درس مجاز حداقل یک زمان پیشنهادی ثبت کنید."); }
        ProfessorTeachingRequestLogic.Submit(        request,clock.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(request);
    }

    public async Task<ICollection<TeachingRequestSummaryDto>> GetSummaryAsync(long termId, CancellationToken cancellationToken)
    {
        await EnsureTermAsync(termId, false, cancellationToken);
        return (await repository.GetSubmittedAsync(termId, cancellationToken)).OrderBy(r => r.ProfessorId).Select(r => new TeachingRequestSummaryDto(r.ProfessorId, r.Professor.User.FirstName + " " + r.Professor.User.LastName, Map(r))).ToList();
    }

    private static TeachingRequestDto Map(ProfessorTeachingRequest request, ICollection<Course>? courses = null) => new(request.Id, request.AcademicTermId, request.Status.ToString(), request.SubmittedAt, request.Courses.OrderBy(c => c.Priority).Select(c =>
    {
        var course = courses?.Single(x => x.Id == c.CourseId) ?? c.Course;
        return new TeachingCourseDto(c.CourseId, course.Code, course.Title, course.Credits, c.Priority);
    }).ToList(), request.Availabilities.Where(a => a.CourseId.HasValue).OrderBy(a => a.CourseId).ThenBy(a => a.DayOfWeek).ThenBy(a => a.StartTime).Select(a => new AvailabilityInput(a.DayOfWeek, a.StartTime, a.EndTime, a.CourseId)).ToList());
}

