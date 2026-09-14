using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetStudentPreRegistration;

/// <summary>
/// Retrieves the pre-registration record for the current authenticated student in the specified academic term.
/// Returns null if no pre-registration exists yet.
/// </summary>
public class GetStudentPreRegistrationQuery : IRequest<StudentPreRegistrationDto?>
{
    public long AcademicTermId { get; set; }
}
