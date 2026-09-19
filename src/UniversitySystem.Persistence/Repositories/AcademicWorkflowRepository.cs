using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.AcademicWorkflow;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;
usingUniversitySystem.Application.Common.Logic;

namespace UniversitySystem.Persistence.Repositories;

/// <summary>خواندن و ثبت رابطه استاد و درس و دریافت تقاضای همان درس‌ها؛ قواعد تصمیم‌گیری در Application است.</summary>
public sealed class AcademicWorkflowRepository(ApplicationDbContext _context) : IAcademicWorkflowRepository
{
    public async Task<ICollection<FinalTeachingOffering>> GetFinalScheduleAsync(long professorId, long termId, CancellationToken ct)
    {
        var offerings = await _context.CourseOfferings.AsNoTracking().Include(o => o.Course).Include(o => o.Schedules).Where(o => o.AcademicTermId == termId && o.IsActive && o.IsFinalized && o.TeachingAssignments.Any(a => a.ProfessorId == professorId)).OrderBy(o => o.Course.Code).ToListAsync(ct);
        return offerings.Select(o => new FinalTeachingOffering(o.Id, o.CourseId, o.Course.Title, o.Capacity, o.Schedules.OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime).Select(s => new UniversitySystem.Application.Features.CourseOfferings.DTOs.CourseOfferingScheduleSlotDto { DayOfWeek = s.DayOfWeek, StartTime = s.StartTime, EndTime = s.EndTime }).ToList())).ToList();
    }
    public Task<bool> ProfessorExistsAsync(long professorId, CancellationToken ct) => _context.Professors.AnyAsync(p => p.Id == professorId && p.User.IsActive, ct);
    public Task<long?> GetProfessorIdAsync(long userId, CancellationToken ct) => _context.Professors.Where(p => p.UserId == userId && p.User.IsActive).Select(p => (long?)p.Id).SingleOrDefaultAsync(ct);
    public Task<bool> TermExistsAsync(long termId, CancellationToken ct) => _context.AcademicTerms.AnyAsync(t => t.Id == termId, ct);
    public async Task<ICollection<long>> GetAllowedCourseIdsAsync(long professorId, CancellationToken ct) => await _context.ProfessorCourses.AsNoTracking().Where(p => p.ProfessorId == professorId).Select(p => p.CourseId).ToListAsync(ct);
    public async Task<ICollection<ProfessorCourseOption>> GetCoursesAsync(long professorId, long termId, CancellationToken ct)
    {
        var courses = await _context.ProfessorCourses.AsNoTracking().Where(p => p.ProfessorId == professorId && p.Course.IsActive).Select(p => new { p.CourseId, p.Course.Code, p.Course.Title, p.Course.Credits }).ToListAsync(ct);
        var demand = await (from item in _context.StudentPreRegistrationItems.AsNoTracking() join request in _context.StudentPreRegistrations.AsNoTracking() on item.StudentPreRegistrationId equals request.Id where request.AcademicTermId == termId && request.Status == RequestStatus.Submitted group item by item.CourseId into g select new { CourseId = g.Key, Count = g.Count() }).ToListAsync(ct);
        return courses.OrderBy(c => c.Code).Select(c => new ProfessorCourseOption(c.CourseId, c.Code, c.Title, c.Credits, demand.FirstOrDefault(d => d.CourseId == c.CourseId)?.Count ?? 0)).ToList();
    }
    public async Task<bool> CoursesExistAsync(ICollection<long> ids, CancellationToken ct) => await _context.Courses.CountAsync(c => ids.Contains(c.Id) && c.IsActive, ct) == ids.Count;
    public async Task<bool> HasCommittedCoursesAsync(long professorId, ICollection<long> ids, CancellationToken ct)
    {
        var requested = await _context.ProfessorTeachingRequestCourses.AnyAsync(c => ids.Contains(c.CourseId) && c.ProfessorTeachingRequest.ProfessorId == professorId && c.ProfessorTeachingRequest.Status == RequestStatus.Submitted && c.ProfessorTeachingRequest.AcademicTerm.IsActive, ct);
        return requested || await _context.TeachingAssignments.AnyAsync(a => a.ProfessorId == professorId && ids.Contains(a.CourseOffering.CourseId) && a.CourseOffering.IsActive && a.CourseOffering.AcademicTerm.IsActive, ct);
    }
    public async Task ReplaceCoursesAsync(long professorId, ICollection<long> ids, CancellationToken ct)
    {
        var existing = await _context.ProfessorCourses.Where(p => p.ProfessorId == professorId).ToListAsync(ct);
        _context.ProfessorCourses.RemoveRange(existing.Where(p => !ids.Contains(p.CourseId)));
        foreach (var id in ids.Where(id => existing.All(p => p.CourseId != id)))
        {
            _context.ProfessorCourses.Add(ProfessorCourseLogic.Create(professorId,id));
        }
    }
    public Task<bool> HasSubmittedProposalAsync(long professorId, long courseId, long termId, CancellationToken ct) => _context.ProfessorTeachingRequests.AnyAsync(r => r.ProfessorId == professorId && r.AcademicTermId == termId && r.Status == RequestStatus.Submitted && r.Courses.Any(c => c.CourseId == courseId) && r.Availabilities.Any(a => a.CourseId == courseId), ct);
}
