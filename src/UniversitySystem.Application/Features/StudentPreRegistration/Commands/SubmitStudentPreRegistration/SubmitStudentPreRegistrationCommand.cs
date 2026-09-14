using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SubmitStudentPreRegistration;

/// <summary>
/// Command to finalize and submit a student's pre-registration draft for a given academic term.
/// </summary>
public sealed record SubmitStudentPreRegistrationCommand(long AcademicTermId)
    : IRequest<StudentPreRegistrationDto>;
