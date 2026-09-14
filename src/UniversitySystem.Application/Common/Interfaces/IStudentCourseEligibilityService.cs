using UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// Service to compute course eligibility for students.
/// Reusable across query and command handlers without duplication of eligibility rules.
/// </summary>
public interface IStudentCourseEligibilityService
{
    /// <summary>
    /// Computes the list of eligible courses for a student for the given academic term.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="academicTermId">The target academic term identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of eligible courses with prerequisite details.</returns>
    Task<IReadOnlyList<EligibleCourseDto>> GetEligibleCoursesAsync(long studentId, long academicTermId, CancellationToken cancellationToken = default);
}
