using MediatR;
using UniversitySystem.Application.Features.Enrollments.DTOs;
using UniversitySystem.Application.Features.Enrollments.Services;

namespace UniversitySystem.Application.Features.Enrollments.Commands.CreateEnrollment;

/// <summary>
/// درخواست «ثبت‌نام قطعی دانشجو در ارائه» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class CreateEnrollmentHandler(EnrollmentService service) : IRequestHandler<CreateEnrollmentCommand, EnrollmentDto>
{
    public Task<EnrollmentDto> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        return service.CreateAsync(request.CourseOfferingId, cancellationToken);
    }
}
