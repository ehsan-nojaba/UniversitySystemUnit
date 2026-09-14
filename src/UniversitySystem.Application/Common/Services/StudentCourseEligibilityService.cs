using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Services;

/// <summary>
/// Implements reusable course eligibility rules for students:
/// 1. Verifies active curriculum for the student's major.
/// 2. Filters out courses already passed.
/// 3. Validates all prerequisites are satisfied (passed).
/// </summary>
public sealed class StudentCourseEligibilityService : IStudentCourseEligibilityService
{
    private readonly IApplicationDbContext _context;

    public StudentCourseEligibilityService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<EligibleCourseDto>> GetEligibleCoursesAsync(
        long studentId,
        long academicTermId,
        CancellationToken cancellationToken = default)
    {
        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);

        if (student is null)
            return [];

        // 1. Resolve active curriculum for student's major
        var curriculum = await _context.Curriculums
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.MajorId == student.MajorId && c.IsActive, cancellationToken);

        if (curriculum is null)
            return [];

        // 2. Fetch active curriculum courses
        var curriculumCourses = await _context.CurriculumCourses
            .AsNoTracking()
            .Where(cc => cc.CurriculumId == curriculum.Id && cc.Course.IsActive)
            .Select(cc => new
            {
                cc.CourseId,
                cc.Course.Code,
                cc.Course.Title,
                cc.Course.Credits,
                cc.RecommendedTerm,
                cc.IsRequired
            })
            .ToListAsync(cancellationToken);

        if (curriculumCourses.Count == 0)
            return [];

        // 3. Gather passed courses from history & enrollments
        var passedFromHistory = await _context.StudentCourseHistories
            .AsNoTracking()
            .Where(h => h.StudentId == student.Id && h.Status == CourseEnrollmentStatus.Passed)
            .Select(h => h.CourseId)
            .ToListAsync(cancellationToken);

        var passedFromEnrollment = await (
            from e in _context.Enrollments.AsNoTracking()
            join co in _context.CourseOfferings on e.CourseOfferingId equals co.Id
            where e.StudentId == student.Id && e.Status == EnrollmentStatus.Completed
            select co.CourseId
        ).ToListAsync(cancellationToken);

        var passedSet = new HashSet<long>(passedFromHistory.Concat(passedFromEnrollment));

        // 4. Filter out already passed courses
        var unpassedCourses = curriculumCourses
            .Where(cc => !passedSet.Contains(cc.CourseId))
            .ToList();

        if (unpassedCourses.Count == 0)
            return [];

        // 5. Gather prerequisites for candidate courses
        var candidateCourseIds = unpassedCourses.Select(c => c.CourseId).ToList();

        var prerequisites = await _context.CoursePrerequisites
            .AsNoTracking()
            .Where(cp => candidateCourseIds.Contains(cp.CourseId))
            .Select(cp => new
            {
                cp.CourseId,
                cp.PrerequisiteCourseId,
                PrerequisiteCode = cp.PrerequisiteCourse.Code,
                PrerequisiteTitle = cp.PrerequisiteCourse.Title
            })
            .ToListAsync(cancellationToken);

        var prereqLookup = prerequisites.ToLookup(p => p.CourseId);

        // 6. Keep courses whose prerequisites are all satisfied
        var result = new List<EligibleCourseDto>();

        foreach (var course in unpassedCourses)
        {
            var coursePrereqs = prereqLookup[course.CourseId].ToList();
            bool allMet = coursePrereqs.All(p => passedSet.Contains(p.PrerequisiteCourseId));

            if (!allMet)
                continue;

            var prereqDtos = coursePrereqs
                .Select(p => new CoursePrerequisiteDto
                {
                    PrerequisiteCourseId = p.PrerequisiteCourseId,
                    Code = p.PrerequisiteCode,
                    Title = p.PrerequisiteTitle
                })
                .ToList();

            result.Add(new EligibleCourseDto
            {
                CourseId = course.CourseId,
                Code = course.Code,
                Title = course.Title,
                Credits = course.Credits,
                RecommendedTerm = course.RecommendedTerm,
                IsRequired = course.IsRequired,
                Prerequisites = prereqDtos
            });
        }

        return result
            .OrderBy(c => c.RecommendedTerm)
            .ThenBy(c => c.Code)
            .ToList();
    }
}
