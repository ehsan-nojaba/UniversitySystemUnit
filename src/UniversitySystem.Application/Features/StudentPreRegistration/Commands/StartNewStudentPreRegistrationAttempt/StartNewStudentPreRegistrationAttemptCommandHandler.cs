using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Services;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.StartNewStudentPreRegistrationAttempt;

/// <summary>
/// درخواست شروع نوبت جدید را به سرویس پیش‌انتخاب می‌سپارد.
/// </summary>
public sealed class StartNewStudentPreRegistrationAttemptCommandHandler(IStudentPreRegistrationService service)
    : IRequestHandler<StartNewStudentPreRegistrationAttemptCommand, StudentPreRegistrationDto>
{
    public Task<StudentPreRegistrationDto> Handle(StartNewStudentPreRegistrationAttemptCommand request, CancellationToken cancellationToken)
    {
        return service.StartNewAttemptAsync(request.AcademicTermId, cancellationToken);
    }
}
