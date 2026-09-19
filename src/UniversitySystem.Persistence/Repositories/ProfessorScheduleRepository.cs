using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>
/// دسترسی EF به برنامه سایر ارائه‌های فعال همان ترم و زمان‌های آزاد استاد برای بررسی تداخل؛ تصمیم آموزشی در سرویس Application انجام می‌شود.
/// </summary>
public sealed class ProfessorScheduleRepository(ApplicationDbContext _context) : IProfessorScheduleRepository
{
    public async Task<ICollection<ProfessorScheduleData>> GetDataAsync(long offeringId, long? additionalProfessorId, CancellationToken cancellationToken)
    {
        var offering = await _context.CourseOfferings.Where(o => o.Id == offeringId).Select(o => new { o.AcademicTermId, o.CourseId }).SingleAsync(cancellationToken);
        var termId = offering.AcademicTermId;
        var ids = await _context.TeachingAssignments.Where(a => a.CourseOfferingId == offeringId).Select(a => a.ProfessorId).ToListAsync(cancellationToken);
        if (additionalProfessorId.HasValue)
        {
            ids.Add(additionalProfessorId.Value);
        }

        var professors = await _context.Professors.AsNoTracking().Where(p => ids.Contains(p.Id)).Select(p => new { p.Id, FullName = p.User.FirstName + " " + p.User.LastName }).ToListAsync(cancellationToken);
        var schedules = await (from a in _context.TeachingAssignments.AsNoTracking() join s in _context.CourseOfferingSchedules.AsNoTracking() on a.CourseOfferingId equals s.CourseOfferingId where ids.Contains(a.ProfessorId) && a.CourseOfferingId != offeringId && a.CourseOffering.AcademicTermId == termId && a.CourseOffering.IsActive select new { a.ProfessorId, s.DayOfWeek, s.StartTime, s.EndTime } ).ToListAsync(cancellationToken);
        var availability = await (from r in _context.ProfessorTeachingRequests.AsNoTracking() join a in _context.ProfessorAvailabilities.AsNoTracking() on r.Id equals a.ProfessorTeachingRequestId where ids.Contains(r.ProfessorId) && r.AcademicTermId == termId && r.Status == RequestStatus.Submitted && a.CourseId == offering.CourseId select new { r.ProfessorId, a.DayOfWeek, a.StartTime, a.EndTime } ).ToListAsync(cancellationToken);
        return professors.Select(p => new ProfessorScheduleData(p.Id, p.FullName, schedules.Where(s => s.ProfessorId == p.Id).Select(s => new CourseOfferingScheduleSlotDto { DayOfWeek = s.DayOfWeek, StartTime = s.StartTime, EndTime = s.EndTime }).ToList(), availability.Where(s => s.ProfessorId == p.Id).Select(s => new CourseOfferingScheduleSlotDto { DayOfWeek = s.DayOfWeek, StartTime = s.StartTime, EndTime = s.EndTime }).ToList())).ToList();
    }
}
