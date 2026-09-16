using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.AdminReports.DTOs;
using UniversitySystem.Application.Features.AdminReports.Repositories;
using UniversitySystem.Application.Features.AdminReports;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>
/// دسترسی EF به تجمیع ظرفیت، ثبت‌نام فعال و تعداد استاد و بازه کلاس برای گزارش آموزش؛ تصمیم آموزشی در سرویس Application انجام می‌شود.
/// </summary>
public sealed class AdminReportRepository(ApplicationDbContext context) : IAdminReportRepository
{
    public async Task<IReadOnlyCollection<OfferingReportDto>> GetOfferingsAsync(long termId, CancellationToken cancellationToken)
    {
        var rows = await context.CourseOfferings.AsNoTracking().Where(o => o.AcademicTermId == termId).OrderBy(o => o.Course.Code)
            .Select(o => new
            {
                o.Id,
                o.CourseId,
                o.Course.Code,
                o.Course.Title,
                o.Capacity,
                o.IsActive,
                Count = o.Enrollments.Count(e => e.Status == EnrollmentStatus.Enrolled),
                Professors = o.TeachingAssignments.Count,
                Schedules = o.Schedules.Count
            }).ToListAsync(cancellationToken);
        return rows.Select(o => new OfferingReportDto(o.Id, o.CourseId, o.Code, o.Title, o.Capacity, o.Count,
            Math.Max(0, o.Capacity - o.Count), o.IsActive, o.Professors, o.Schedules)).ToList();
    }
}
