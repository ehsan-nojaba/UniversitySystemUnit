using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Services;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SaveStudentPreRegistration;

/// <summary>
/// درخواست «ذخیره یا ویرایش پیش‌نویس پیش‌انتخاب» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class SaveStudentPreRegistrationCommandHandler(IStudentPreRegistrationService service)
    : IRequestHandler<SaveStudentPreRegistrationCommand, StudentPreRegistrationDto>
{
    public Task<StudentPreRegistrationDto> Handle(SaveStudentPreRegistrationCommand request, CancellationToken cancellationToken)
    {
        return service.SaveDraftAsync(request.AcademicTermId, request.Courses, cancellationToken);
    }
}
