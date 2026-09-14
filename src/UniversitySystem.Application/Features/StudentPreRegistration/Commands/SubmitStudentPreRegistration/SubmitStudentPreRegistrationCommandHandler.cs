using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Services;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SubmitStudentPreRegistration;

public sealed class SubmitStudentPreRegistrationCommandHandler(IStudentPreRegistrationService service)
    : IRequestHandler<SubmitStudentPreRegistrationCommand, StudentPreRegistrationDto>
{
    public Task<StudentPreRegistrationDto> Handle(SubmitStudentPreRegistrationCommand request, CancellationToken cancellationToken)
        => service.SubmitAsync(request.AcademicTermId, cancellationToken);
}
