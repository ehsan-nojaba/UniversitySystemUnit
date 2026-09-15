using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;

public sealed class CourseOfferingRepository(ApplicationDbContext context) : ICourseOfferingRepository
{
    public async Task<AcademicTerm?> GetAcademicTermAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (
            from t in context.AcademicTerms.AsNoTracking()
            where t.Id == termId
            select t
        ).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Course?> GetCourseAsync(long courseId, CancellationToken cancellationToken = default)
    {
        return await (
            from c in context.Courses.AsNoTracking()
            where c.Id == courseId
            select c
        ).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CourseOffering?> GetByIdWithCourseAsync(long id, CancellationToken cancellationToken = default)
    {
        return await context.CourseOfferings
            .Include(co => co.Course)
            .FirstOrDefaultAsync(co => co.Id == id, cancellationToken);
    }

    public async Task<bool> OfferingExistsAsync(long termId, long courseId, CancellationToken cancellationToken = default)
    {
        return await (
            from co in context.CourseOfferings.AsNoTracking()
            where co.AcademicTermId == termId && co.CourseId == courseId
            select co.Id
        ).AnyAsync(cancellationToken);
    }

    public async Task<ICollection<CourseOfferingDto>> GetOfferingsByTermAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (
            from co in context.CourseOfferings.AsNoTracking()
            join c in context.Courses.AsNoTracking() on co.CourseId equals c.Id
            where co.AcademicTermId == termId
            orderby c.Code
            select new CourseOfferingDto
            {
                CourseOfferingId = co.Id,
                AcademicTermId = co.AcademicTermId,
                CourseId = co.CourseId,
                CourseCode = c.Code,
                CourseTitle = c.Title,
                Capacity = co.Capacity,
                IsActive = co.IsActive
            }
        ).ToListAsync(cancellationToken);
    }

    public async Task<CourseOffering?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await context.CourseOfferings.FirstOrDefaultAsync(co => co.Id == id, cancellationToken);
    }

    public async Task<ICollection<CourseOfferingScheduleDto>> GetSchedulesByOfferingIdAsync(long offeringId, CancellationToken cancellationToken = default)
    {
        return await (
            from s in context.CourseOfferingSchedules.AsNoTracking()
            where s.CourseOfferingId == offeringId
            orderby s.DayOfWeek, s.StartTime
            select new CourseOfferingScheduleDto
            {
                Id = s.Id,
                CourseOfferingId = s.CourseOfferingId,
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            }
        ).ToListAsync(cancellationToken);
    }

    public async Task SaveSchedulesAsync(long offeringId, IReadOnlyCollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default)
    {
        var existing = await context.CourseOfferingSchedules
            .Where(s => s.CourseOfferingId == offeringId)
            .ToListAsync(cancellationToken);

        context.CourseOfferingSchedules.RemoveRange(existing);

        foreach (var slot in slots)
        {
            context.CourseOfferingSchedules.Add(new CourseOfferingSchedule(offeringId, slot.DayOfWeek, slot.StartTime, slot.EndTime));
        }
    }

    public void Add(CourseOffering offering) => context.CourseOfferings.Add(offering);
}
