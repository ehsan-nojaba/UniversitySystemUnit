using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Services;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SaveStudentPreRegistration;

public sealed class SaveStudentPreRegistrationCommandHandler(IStudentPreRegistrationService service)
    : IRequestHandler<SaveStudentPreRegistrationCommand, StudentPreRegistrationDto>
{
    public Task<StudentPreRegistrationDto> Handle(SaveStudentPreRegistrationCommand request, CancellationToken cancellationToken)
        => service.SaveDraftAsync(request.AcademicTermId, request.Courses, cancellationToken);
}
