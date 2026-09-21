using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Services;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.Application.Features.AcademicWorkflow;

/// <summary>آموزش درس‌های مجاز استاد را مشخص می‌کند؛ استاد تقاضاها را می‌بیند و آموزش پس از بررسی زمان و ظرفیت، کلاس را نهایی می‌کند.</summary>
public sealed class AcademicWorkflowService(IAcademicWorkflowRepository repository, ICurrentUserService currentUser, IUnitOfWork unitOfWork, IEnrollmentRepository enrollments, IProfessorScheduleConflictChecker conflicts)
{
    public async Task<ICollection<FinalTeachingOffering>> GetMyFinalScheduleAsync(long termId, CancellationToken ct)
    {
        if (!long.TryParse(currentUser.UserId, out var userId)) { throw new UnauthorizedAccessException(); }
        var professorId = await repository.GetProfessorIdAsync(userId, ct) ?? throw new NotFoundException("پروفایل استاد پیدا نشد.");
        if (!await repository.TermExistsAsync(termId, ct)) { throw new NotFoundException("ترم پیدا نشد."); }
        return await repository.GetFinalScheduleAsync(professorId, termId, ct);
    }
    public async Task<ICollection<ProfessorCourseOption>> GetMyCoursesAsync(long termId, CancellationToken ct)
    {
        if (!long.TryParse(currentUser.UserId, out var userId)) { throw new UnauthorizedAccessException(); }
        var professorId = await repository.GetProfessorIdAsync(userId, ct) ?? throw new NotFoundException("پروفایل استاد پیدا نشد.");
        if (!await repository.TermExistsAsync(termId, ct)) { throw new NotFoundException("ترم پیدا نشد."); }
        return await repository.GetCoursesAsync(professorId, termId, ct);
    }
    public async Task<ICollection<long>> GetAllowedAsync(long professorId, CancellationToken ct)
    {
        if (!await repository.ProfessorExistsAsync(professorId, ct)) { throw new NotFoundException("استاد فعال پیدا نشد."); }
        return await repository.GetAllowedCourseIdsAsync(professorId, ct);
    }
    public async Task<ICollection<long>> SaveAllowedAsync(long professorId, ICollection<long> ids, CancellationToken ct)
    {
        var previous = await GetAllowedAsync(professorId, ct);
        if (ids is null || ids.Any(id => id <= 0) || ids.Distinct().Count() != ids.Count || !await repository.CoursesExistAsync(ids, ct)) { throw new BusinessException("فهرست درس‌ها باید شامل درس‌های فعال، معتبر و بدون تکرار باشد."); }
        var removed = previous.Except(ids).ToList();
        if (removed.Count > 0 && await repository.HasCommittedCoursesAsync(professorId, removed, ct)) { throw new BusinessException("ارتباط درسی که درخواست ارسال‌شده یا کلاس فعال دارد قابل حذف نیست."); }
        await repository.ReplaceCoursesAsync(professorId, ids, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return ids;
    }
    public Task<bool> SetFinalizedAsync(long offeringId, bool finalize, CancellationToken ct)
    {
        return enrollments.ExecuteSerializableAsync(async () =>
        {
            var offering = await enrollments.GetOfferingAsync(offeringId, ct) ?? throw new NotFoundException("ارائه پیدا نشد.");
            if (!offering.AcademicTerm.IsActive) { throw new BusinessException("ترم غیرفعال قابل تغییر نیست."); }
            if (finalize)
            {
                if (!offering.IsActive || !offering.Course.IsActive || offering.Capacity <= 0 || !offering.TeachingAssignments.Any() || !offering.Schedules.Any()) { throw new BusinessException("برای نهایی‌کردن، درس و ارائه باید فعال و ظرفیت، استاد و زمان کلاس مشخص باشند."); }
                foreach (var assignment in offering.TeachingAssignments)
                {
                    var allowed = await repository.GetAllowedCourseIdsAsync(assignment.ProfessorId, ct);
                    if (!assignment.Professor.User.IsActive || !allowed.Contains(offering.CourseId) || !await repository.HasSubmittedProposalAsync(assignment.ProfessorId, offering.CourseId, offering.AcademicTermId, ct)) { throw new BusinessException("هر استاد باید برای همین درس مجاز باشد و پیشنهاد زمانی آن را ارسال کرده باشد."); }
                }
                await conflicts.CheckConflictsAsync(offering.Id, offering.Schedules.Select(s => new CourseOfferingScheduleSlotDto { DayOfWeek = s.DayOfWeek, StartTime = s.StartTime, EndTime = s.EndTime }).ToList(), ct);
                CourseOfferingLogic.FinalizePlanning(offering);
            }
            else
            {
                if (await enrollments.GetEnrollmentCountAsync(offering.Id, ct) > 0) { throw new BusinessException("برای این کلاس ثبت‌نام انجام شده و بازگشت به برنامه‌ریزی مجاز نیست."); }
                CourseOfferingLogic.ReopenPlanning(offering);
            }
            await unitOfWork.SaveChangesAsync(ct);
            return offering.IsFinalized;
        }, ct);
    }
}
