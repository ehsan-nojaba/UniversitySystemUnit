using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.AdminPlanning.Repositories;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>
/// دسترسی EF به اطلاعات ترم، مشخصات درس، تعداد تقاضا و علاقه استاد برای برنامه‌ریزی؛ تصمیم آموزشی در سرویس Application انجام می‌شود.
/// </summary>
public sealed class AdminPlanningRepository(ApplicationDbContext _context) : IAdminPlanningRepository
{
    public async Task<AcademicTermInfoModel?> GetAcademicTermInfoAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await _context.AcademicTerms.AsNoTracking().Where(t => t.Id == termId).Select(t => new AcademicTermInfoModel { Id = t.Id, Code = t.Code, Title = t.Title }).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<CourseDemandCountModel>> GetSubmittedStudentDemandCountsAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (from item in _context.StudentPreRegistrationItems.AsNoTracking() join preReg in _context.StudentPreRegistrations.AsNoTracking() on item.StudentPreRegistrationId equals preReg.Id where preReg.AcademicTermId == termId && preReg.Status == RequestStatus.Submitted group item by item.CourseId into g select new CourseDemandCountModel { CourseId = g.Key, DemandCount = g.Count() } ).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<ProfessorCourseInterestModel>> GetSubmittedProfessorInterestsAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (from reqCourse in _context.ProfessorTeachingRequestCourses.AsNoTracking() join req in _context.ProfessorTeachingRequests.AsNoTracking() on reqCourse.ProfessorTeachingRequestId equals req.Id join prof in _context.Professors.AsNoTracking() on req.ProfessorId equals prof.Id join user in _context.Users.AsNoTracking() on prof.UserId equals user.Id where req.AcademicTermId == termId && req.Status == RequestStatus.Submitted select new ProfessorCourseInterestModel { CourseId = reqCourse.CourseId, ProfessorId = req.ProfessorId, ProfessorFullName = user.FirstName + " " + user.LastName, Priority = reqCourse.Priority } ).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<CourseInfoModel>> GetCoursesByIdsAsync(long[] courseIds, CancellationToken cancellationToken = default)
    {
        return await _context.Courses.AsNoTracking().Where(c => courseIds.Contains(c.Id)).Select(c => new CourseInfoModel { Id = c.Id, Code = c.Code, Title = c.Title, Credits = c.Credits }).ToListAsync(cancellationToken);
    }
}
