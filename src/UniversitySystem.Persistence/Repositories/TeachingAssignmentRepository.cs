using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;

public sealed class TeachingAssignmentRepository(ApplicationDbContext context) : ITeachingAssignmentRepository
{
    public async Task<CourseOffering?> GetCourseOfferingAsync(long offeringId, CancellationToken cancellationToken = default)
    {
        return await (
            from co in context.CourseOfferings.AsNoTracking()
            where co.Id == offeringId
            select co
        ).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Professor?> GetProfessorAsync(long professorId, CancellationToken cancellationToken = default)
    {
        return await (
            from p in context.Professors.AsNoTracking()
            where p.Id == professorId
            select p
        ).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string> GetProfessorFullNameAsync(long professorId, CancellationToken cancellationToken = default)
    {
        var name = await (
            from p in context.Professors.AsNoTracking()
            join u in context.Users.AsNoTracking() on p.UserId equals u.Id
            where p.Id == professorId
            select u.FirstName + " " + u.LastName
        ).FirstOrDefaultAsync(cancellationToken);

        return name ?? string.Empty;
    }

    public async Task<bool> IsProfessorAssignedAsync(long offeringId, long professorId, CancellationToken cancellationToken = default)
    {
        return await (
            from ta in context.TeachingAssignments.AsNoTracking()
            where ta.CourseOfferingId == offeringId && ta.ProfessorId == professorId
            select ta.Id
        ).AnyAsync(cancellationToken);
    }

    public async Task<TeachingAssignment?> GetAssignmentAsync(long offeringId, long professorId, CancellationToken cancellationToken = default)
    {
        return await context.TeachingAssignments
            .FirstOrDefaultAsync(ta => ta.CourseOfferingId == offeringId && ta.ProfessorId == professorId, cancellationToken);
    }

    public async Task<(bool Requested, int? Priority)> GetProfessorCourseRequestInfoAsync(long professorId, long termId, long courseId, CancellationToken cancellationToken = default)
    {
        var req = await (
            from rc in context.ProfessorTeachingRequestCourses.AsNoTracking()
            join r in context.ProfessorTeachingRequests.AsNoTracking() on rc.ProfessorTeachingRequestId equals r.Id
            where r.ProfessorId == professorId
               && r.AcademicTermId == termId
               && r.Status == RequestStatus.Submitted
               && rc.CourseId == courseId
            select (int?)rc.Priority
        ).FirstOrDefaultAsync(cancellationToken);

        return req.HasValue ? (true, req) : (false, null);
    }

    public async Task<ICollection<TeachingAssignmentDto>> GetAssignmentsByOfferingIdAsync(long offeringId, CancellationToken cancellationToken = default)
    {
        var offering = await (
            from co in context.CourseOfferings.AsNoTracking()
            where co.Id == offeringId
            select new { co.AcademicTermId, co.CourseId }
        ).FirstOrDefaultAsync(cancellationToken);

        if (offering is null) return [];

        var assignments = await (
            from ta in context.TeachingAssignments.AsNoTracking()
            join p in context.Professors.AsNoTracking() on ta.ProfessorId equals p.Id
            join u in context.Users.AsNoTracking() on p.UserId equals u.Id
            where ta.CourseOfferingId == offeringId
            select new
            {
                ta.Id,
                ta.ProfessorId,
                ProfessorFullName = u.FirstName + " " + u.LastName,
                ta.AssignedAt
            }
        ).ToListAsync(cancellationToken);

        var profRequests = await (
            from rc in context.ProfessorTeachingRequestCourses.AsNoTracking()
            join r in context.ProfessorTeachingRequests.AsNoTracking() on rc.ProfessorTeachingRequestId equals r.Id
            where r.AcademicTermId == offering.AcademicTermId
               && r.Status == RequestStatus.Submitted
               && rc.CourseId == offering.CourseId
            select new { r.ProfessorId, rc.Priority }
        ).ToListAsync(cancellationToken);

        var requestMap = profRequests.ToDictionary(r => r.ProfessorId, r => r.Priority);

        return (
            from a in assignments
            let priority = requestMap.ContainsKey(a.ProfessorId) ? (int?)requestMap[a.ProfessorId] : null
            select new TeachingAssignmentDto
            {
                TeachingAssignmentId = a.Id,
                ProfessorId = a.ProfessorId,
                ProfessorFullName = a.ProfessorFullName,
                AssignedAt = a.AssignedAt,
                RequestedThisCourse = priority.HasValue,
                RequestedPriority = priority
            }
        ).ToList();
    }

    public void Add(TeachingAssignment assignment) => context.TeachingAssignments.Add(assignment);

    public void Remove(TeachingAssignment assignment) => context.TeachingAssignments.Remove(assignment);
}
