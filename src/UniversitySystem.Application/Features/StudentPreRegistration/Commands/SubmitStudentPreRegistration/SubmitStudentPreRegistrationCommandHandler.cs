using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Services;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SubmitStudentPreRegistration;

/// <summary>
/// درخواست «ارسال نهایی پیش‌انتخاب» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class SubmitStudentPreRegistrationCommandHandler(IStudentPreRegistrationService service)
    : IRequestHandler<SubmitStudentPreRegistrationCommand, StudentPreRegistrationDto>
{
    public Task<StudentPreRegistrationDto> Handle(SubmitStudentPreRegistrationCommand request, CancellationToken cancellationToken)
    {
        return service.SubmitAsync(request.AcademicTermId, cancellationToken);
    }
}
