using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.AdminPreRegistration.Repositories;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;

public sealed class AdminPreRegistrationRepository(ApplicationDbContext context) : IAdminPreRegistrationRepository
{
    public async Task<bool> AcademicTermExistsAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (
            from t in context.AcademicTerms.AsNoTracking()
            where t.Id == termId
            select t.Id
        ).AnyAsync(cancellationToken);
    }

    public async Task<ICollection<CourseDemandAggregatedModel>> GetCourseDemandSummaryAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (
            from item in context.StudentPreRegistrationItems.AsNoTracking()
            join preReg in context.StudentPreRegistrations.AsNoTracking() on item.StudentPreRegistrationId equals preReg.Id
            join course in context.Courses.AsNoTracking() on item.CourseId equals course.Id
            where preReg.AcademicTermId == termId && preReg.Status == RequestStatus.Submitted
            group new { item, course } by new { item.CourseId, course.Code, course.Title, course.Credits } into g
            orderby g.Count() descending, g.Key.Code
            select new CourseDemandAggregatedModel
            {
                CourseId = g.Key.CourseId,
                CourseCode = g.Key.Code,
                CourseTitle = g.Key.Title,
                Credits = g.Key.Credits,
                StudentCount = g.Count(),
                AveragePriority = g.Average(x => (double)x.item.Priority)
            }
        ).ToListAsync(cancellationToken);
    }
}
