using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SaveStudentPreRegistration;

/// <summary>
/// Command to create or update a student's pre-registration draft for an academic term.
/// The student identity is derived from the current user security context.
/// </summary>
public class SaveStudentPreRegistrationCommand : IRequest<StudentPreRegistrationDto>
{
    public long AcademicTermId { get; set; }
    public ICollection<SelectedCourseItemDto> Courses { get; set; } = [];
}
