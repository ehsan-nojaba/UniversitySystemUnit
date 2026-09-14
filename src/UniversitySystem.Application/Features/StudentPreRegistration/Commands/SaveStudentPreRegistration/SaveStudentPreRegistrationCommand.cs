using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SaveStudentPreRegistration;

/// <summary>
/// Command to create or update a student's pre-registration draft for an academic term.
/// The student identity is derived from the current user security context.
/// </summary>
/// <param name="AcademicTermId">Target academic term identifier.</param>
/// <param name="Courses">List of chosen courses with priority rankings.</param>
public sealed record SaveStudentPreRegistrationCommand(
    long AcademicTermId,
    IReadOnlyList<SelectedCourseItemDto> Courses
) : IRequest<StudentPreRegistrationDto>;
