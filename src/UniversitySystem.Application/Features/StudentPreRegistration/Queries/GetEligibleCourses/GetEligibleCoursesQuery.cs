using MediatR;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

/// <summary>
/// Query to retrieve all eligible courses for the currently authenticated student for an academic term.
/// The student identity is obtained from the security context, not the request parameters.
/// </summary>
public class GetEligibleCoursesQuery : IRequest<ICollection<EligibleCourseDto>>
{
    public long AcademicTermId { get; set; }
}
