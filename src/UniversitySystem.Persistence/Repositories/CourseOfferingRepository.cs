using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Persistence.Data;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>
/// دسترسی EF به مشخصات ارائه، درس، ترم و بازه‌های کلاس؛ تصمیم آموزشی در سرویس Application انجام می‌شود.
/// </summary>
public sealed class CourseOfferingRepository(ApplicationDbContext _context) : ICourseOfferingRepository
{
    public async Task<AcademicTerm?> GetAcademicTermAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await _context.AcademicTerms.AsNoTracking().FirstOrDefaultAsync(t => t.Id == termId, cancellationToken);
    }

    public async Task<Course?> GetCourseAsync(long courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == courseId, cancellationToken);
    }

    public async Task<CourseOffering?> GetByIdWithCourseAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.CourseOfferings.Include(co => co.Course).FirstOrDefaultAsync(co => co.Id == id, cancellationToken);
    }

    public async Task<bool> OfferingExistsAsync(long termId, long courseId, CancellationToken cancellationToken = default)
    {
        return await _context.CourseOfferings.AsNoTracking().AnyAsync(co => co.AcademicTermId == termId && co.CourseId == courseId, cancellationToken);
    }

    public async Task<ICollection<CourseOfferingDto>> GetOfferingsByTermAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (from co in _context.CourseOfferings.AsNoTracking() join c in _context.Courses.AsNoTracking() on co.CourseId equals c.Id where co.AcademicTermId == termId orderby c.Code select new CourseOfferingDto { CourseOfferingId = co.Id, AcademicTermId = co.AcademicTermId, CourseId = co.CourseId, CourseCode = c.Code, CourseTitle = c.Title, Capacity = co.Capacity, IsActive = co.IsActive, IsFinalized = co.IsFinalized } ).ToListAsync(cancellationToken);
    }

    public async Task<CourseOffering?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.CourseOfferings.FirstOrDefaultAsync(co => co.Id == id, cancellationToken);
    }

    public async Task<ICollection<CourseOfferingScheduleDto>> GetSchedulesByOfferingIdAsync(long offeringId, CancellationToken cancellationToken = default)
    {
        return await _context.CourseOfferingSchedules.AsNoTracking().Where(s => s.CourseOfferingId == offeringId).OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime).Select(s => new CourseOfferingScheduleDto { Id = s.Id, CourseOfferingId = s.CourseOfferingId, DayOfWeek = s.DayOfWeek, StartTime = s.StartTime, EndTime = s.EndTime }).ToListAsync(cancellationToken);
    }

    public async Task SaveSchedulesAsync(long offeringId, ICollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default)
    {
        var existing = await _context.CourseOfferingSchedules.Where(s => s.CourseOfferingId == offeringId).ToListAsync(cancellationToken);
        _context.CourseOfferingSchedules.RemoveRange(existing);
        foreach (var slot in slots)
        {
            _context.CourseOfferingSchedules.Add(CourseOfferingScheduleLogic.Create(offeringId,slot.DayOfWeek,slot.StartTime,slot.EndTime));
        }
    }

    public void Add(CourseOffering offering) => _context.CourseOfferings.Add(offering);
}

