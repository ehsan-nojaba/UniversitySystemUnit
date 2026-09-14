using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.AdminPlanning.Repositories;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;

public sealed class AdminPlanningRepository(ApplicationDbContext context) : IAdminPlanningRepository
{
    public async Task<AcademicTermInfoModel?> GetAcademicTermInfoAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (
            from t in context.AcademicTerms.AsNoTracking()
            where t.Id == termId
            select new AcademicTermInfoModel { Id = t.Id, Code = t.Code, Title = t.Title }
        ).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<CourseDemandCountModel>> GetSubmittedStudentDemandCountsAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (
            from item in context.StudentPreRegistrationItems.AsNoTracking()
            join preReg in context.StudentPreRegistrations.AsNoTracking() on item.StudentPreRegistrationId equals preReg.Id
            where preReg.AcademicTermId == termId && preReg.Status == RequestStatus.Submitted
            group item by item.CourseId into g
            select new CourseDemandCountModel { CourseId = g.Key, DemandCount = g.Count() }
        ).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<ProfessorCourseInterestModel>> GetSubmittedProfessorInterestsAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (
            from reqCourse in context.ProfessorTeachingRequestCourses.AsNoTracking()
            join req in context.ProfessorTeachingRequests.AsNoTracking() on reqCourse.ProfessorTeachingRequestId equals req.Id
            join prof in context.Professors.AsNoTracking() on req.ProfessorId equals prof.Id
            join user in context.Users.AsNoTracking() on prof.UserId equals user.Id
            where req.AcademicTermId == termId && req.Status == RequestStatus.Submitted
            select new ProfessorCourseInterestModel
            {
                CourseId = reqCourse.CourseId,
                ProfessorId = req.ProfessorId,
                ProfessorFullName = user.FirstName + " " + user.LastName,
                Priority = reqCourse.Priority
            }
        ).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<CourseInfoModel>> GetCoursesByIdsAsync(long[] courseIds, CancellationToken cancellationToken = default)
    {
        return await (
            from c in context.Courses.AsNoTracking()
            where courseIds.Contains(c.Id)
            select new CourseInfoModel { Id = c.Id, Code = c.Code, Title = c.Title, Credits = c.Credits }
        ).ToListAsync(cancellationToken);
    }
}
