using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>
/// داده چارت فعال، درس‌های فعال، سوابق قبولی و پیش‌نیازها را از دیتابیس دریافت می‌کند؛ تصمیم نهایی انتخاب با سرویس Application است.
/// </summary>
public sealed class StudentEligibilityRepository : IStudentEligibilityRepository
{
    private readonly UniversitySystem.Persistence.Data.ApplicationDbContext _context;
    public StudentEligibilityRepository(UniversitySystem.Persistence.Data.ApplicationDbContext _context)
    {
        this._context = _context;
    }

    public async Task<EligibilityData> GetDataAsync(long studentId, CancellationToken cancellationToken = default)
    {
        var student = await _context.Students.AsNoTracking().Where(s => s.Id == studentId && s.Major.IsActive).Select(s => new { s.Id, s.MajorId, DepartmentId = s.Major.DepartmentId }).FirstOrDefaultAsync(cancellationToken);
        if (student is null)
        {
            return new EligibilityData([], new HashSet<long>());
        }

        // 1. گروه آموزشی رشته را پیدا می‌کنیم تا کاتالوگ درس‌های همان گروه مشخص شود.
        var departmentCourseIds = await _context.CurriculumCourses.AsNoTracking().Where(cc => cc.Course.IsActive && cc.Curriculum.IsActive && cc.Curriculum.Major.DepartmentId == student.DepartmentId).Select(cc => cc.CourseId).Distinct().ToListAsync(cancellationToken);
        if (departmentCourseIds.Count == 0)
        {
            return new EligibilityData([], new HashSet<long>());
        }

        // 2. از کاتالوگ گروه فقط چارت فعال رشته/گرایش خود دانشجو را نگه می‌داریم.
        var curriculum = await _context.Curriculums.AsNoTracking().Where(c => c.MajorId == student.MajorId && c.IsActive).OrderByDescending(c => c.Id).FirstOrDefaultAsync(cancellationToken);
        if (curriculum is null)
        {
            return new EligibilityData([], new HashSet<long>());
        }

        // 3. Fetch active courses from the exact major curriculum.
        var curriculumCourses = await _context.CurriculumCourses.AsNoTracking().Where(cc => cc.CurriculumId == curriculum.Id && departmentCourseIds.Contains(cc.CourseId) && cc.Course.IsActive).Select(cc => new { cc.CourseId, cc.Course.Code, cc.Course.Title, cc.Course.Credits, cc.RecommendedTerm, cc.IsRequired }).ToListAsync(cancellationToken);
        if (curriculumCourses.Count == 0)
        {
            return new EligibilityData([], new HashSet<long>());
        }

        // 4. Gather passed courses from history & enrollments
        var passedFromHistory = await _context.StudentCourseHistories.AsNoTracking().Where(h => h.StudentId == student.Id && h.Status == CourseEnrollmentStatus.Passed).Select(h => h.CourseId).ToListAsync(cancellationToken);
        var passedFromEnrollment = await (from e in _context.Enrollments.AsNoTracking() join co in _context.CourseOfferings on e.CourseOfferingId equals co.Id where e.StudentId == student.Id && e.Status == EnrollmentStatus.Completed select co.CourseId).ToListAsync(cancellationToken);
        var passedSet = new HashSet<long>(passedFromHistory.Concat(passedFromEnrollment));
        var candidateCourseIds = curriculumCourses.Select(c => c.CourseId).ToList();
        var prerequisites = await _context.CoursePrerequisites.AsNoTracking().Where(cp => candidateCourseIds.Contains(cp.CourseId)).Select(cp => new { cp.CourseId, cp.PrerequisiteCourseId, PrerequisiteCode = cp.PrerequisiteCourse.Code, PrerequisiteTitle = cp.PrerequisiteCourse.Title }).ToListAsync(cancellationToken);
        var prereqLookup = prerequisites.ToLookup(p => p.CourseId);
        return new EligibilityData(curriculumCourses.Select(course => new EligibleCourseDto { CourseId = course.CourseId, Code = course.Code, Title = course.Title, Credits = course.Credits, RecommendedTerm = course.RecommendedTerm, IsRequired = course.IsRequired, Prerequisites = prereqLookup[course.CourseId].Select(p => new CoursePrerequisiteDto { PrerequisiteCourseId = p.PrerequisiteCourseId, Code = p.PrerequisiteCode, Title = p.PrerequisiteTitle }).ToList() }).ToList(), passedSet);
    }
}
