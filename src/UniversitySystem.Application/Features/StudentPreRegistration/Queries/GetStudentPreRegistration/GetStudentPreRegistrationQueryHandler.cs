using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Services;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetStudentPreRegistration;

public sealed class GetStudentPreRegistrationQueryHandler(IStudentPreRegistrationService service)
    : IRequestHandler<GetStudentPreRegistrationQuery, StudentPreRegistrationDto?>
{
    public Task<StudentPreRegistrationDto?> Handle(GetStudentPreRegistrationQuery request, CancellationToken cancellationToken)
        => service.GetByTermAsync(request.AcademicTermId, cancellationToken);
}
