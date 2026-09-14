using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;
using UniversitySystem.Application.Features.StudentPreRegistration.Services;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

public sealed class GetEligibleCoursesQueryHandler(IStudentPreRegistrationService service)
    : IRequestHandler<GetEligibleCoursesQuery, ICollection<EligibleCourseDto>>
{
    public Task<ICollection<EligibleCourseDto>> Handle(GetEligibleCoursesQuery request, CancellationToken cancellationToken)
        => service.GetEligibleCoursesAsync(request.AcademicTermId, cancellationToken);
}
