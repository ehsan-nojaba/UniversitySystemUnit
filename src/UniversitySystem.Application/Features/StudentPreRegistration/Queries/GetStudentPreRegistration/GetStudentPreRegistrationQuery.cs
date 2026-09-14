using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetStudentPreRegistration;

/// <summary>
/// Retrieves the pre-registration record for the current authenticated student in the specified academic term.
/// Returns null if no pre-registration exists yet.
/// </summary>
/// <param name="AcademicTermId">Target academic term identifier.</param>
public sealed record GetStudentPreRegistrationQuery(long AcademicTermId) : IRequest<StudentPreRegistrationDto?>;
