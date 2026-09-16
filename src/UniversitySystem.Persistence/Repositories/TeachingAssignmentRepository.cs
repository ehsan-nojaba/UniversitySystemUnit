using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>
/// دسترسی EF به تخصیص استاد، پروفایل استاد و اطلاعات علاقه قبلی او به درس؛ تصمیم آموزشی در سرویس Application انجام می‌شود.
/// </summary>
public sealed class TeachingAssignmentRepository(ApplicationDbContext _context) : ITeachingAssignmentRepository
{
    public async Task<CourseOffering?> GetCourseOfferingAsync(long offeringId, CancellationToken cancellationToken = default)
    {
        return await _context.CourseOfferings.AsNoTracking().FirstOrDefaultAsync(co => co.Id == offeringId, cancellationToken);
    }

    public async Task<Professor?> GetProfessorAsync(long professorId, CancellationToken cancellationToken = default)
    {
        return await _context.Professors.AsNoTracking().FirstOrDefaultAsync(p => p.Id == professorId, cancellationToken);
    }

    public async Task<string> GetProfessorFullNameAsync(long professorId, CancellationToken cancellationToken = default)
    {
        var name = await (from p in _context.Professors.AsNoTracking() join u in _context.Users.AsNoTracking() on p.UserId equals u.Id where p.Id == professorId select u.FirstName + " " + u.LastName).FirstOrDefaultAsync(cancellationToken);
        return name ?? string.Empty;
    }

    public async Task<bool> IsProfessorAssignedAsync(long offeringId, long professorId, CancellationToken cancellationToken = default)
    {
        return await _context.TeachingAssignments.AsNoTracking().AnyAsync(ta => ta.CourseOfferingId == offeringId && ta.ProfessorId == professorId, cancellationToken);
    }

    public async Task<TeachingAssignment?> GetAssignmentAsync(long offeringId, long professorId, CancellationToken cancellationToken = default)
    {
        return await _context.TeachingAssignments.FirstOrDefaultAsync(ta => ta.CourseOfferingId == offeringId && ta.ProfessorId == professorId, cancellationToken);
    }

    public async Task<(bool Requested, int? Priority)> GetProfessorCourseRequestInfoAsync(long professorId, long termId, long courseId, CancellationToken cancellationToken = default)
    {
        var req = await (from rc in _context.ProfessorTeachingRequestCourses.AsNoTracking() join r in _context.ProfessorTeachingRequests.AsNoTracking() on rc.ProfessorTeachingRequestId equals r.Id where r.ProfessorId == professorId && r.AcademicTermId == termId && r.Status == RequestStatus.Submitted && rc.CourseId == courseId select (int?)rc.Priority).FirstOrDefaultAsync(cancellationToken);
        return req.HasValue ? (true, req) : (false, null);
    }

    public async Task<ICollection<TeachingAssignmentDto>> GetAssignmentsByOfferingIdAsync(long offeringId, CancellationToken cancellationToken = default)
    {
        var offering = await _context.CourseOfferings.AsNoTracking().Where(co => co.Id == offeringId).Select(co => new { co.AcademicTermId, co.CourseId }).FirstOrDefaultAsync(cancellationToken);
        if (offering is null)
        {
            return[];
        }

        var assignments = await (from ta in _context.TeachingAssignments.AsNoTracking() join p in _context.Professors.AsNoTracking() on ta.ProfessorId equals p.Id join u in _context.Users.AsNoTracking() on p.UserId equals u.Id where ta.CourseOfferingId == offeringId select new { ta.Id, ta.ProfessorId, ProfessorFullName = u.FirstName + " " + u.LastName, ta.AssignedAt } ).ToListAsync(cancellationToken);
        var profRequests = await (from rc in _context.ProfessorTeachingRequestCourses.AsNoTracking() join r in _context.ProfessorTeachingRequests.AsNoTracking() on rc.ProfessorTeachingRequestId equals r.Id where r.AcademicTermId == offering.AcademicTermId && r.Status == RequestStatus.Submitted && rc.CourseId == offering.CourseId select new { r.ProfessorId, rc.Priority } ).ToListAsync(cancellationToken);
        var requestMap = profRequests.ToDictionary(r => r.ProfessorId, r => r.Priority);
        return assignments.Select(a => new TeachingAssignmentDto { TeachingAssignmentId = a.Id, ProfessorId = a.ProfessorId, ProfessorFullName = a.ProfessorFullName, AssignedAt = a.AssignedAt, RequestedThisCourse = requestMap.ContainsKey(a.ProfessorId), RequestedPriority = requestMap.TryGetValue(a.ProfessorId, out var priority) ? priority : null }).ToList();
    }

    public void Add(TeachingAssignment assignment) => _context.TeachingAssignments.Add(assignment);
    public void Remove(TeachingAssignment assignment) => _context.TeachingAssignments.Remove(assignment);
}
