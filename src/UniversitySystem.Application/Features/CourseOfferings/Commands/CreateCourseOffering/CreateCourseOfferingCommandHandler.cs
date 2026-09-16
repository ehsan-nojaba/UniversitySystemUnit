using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Services;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.CreateCourseOffering;

/// <summary>
/// درخواست «ایجاد ارائه درس با ظرفیت مشخص» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class CreateCourseOfferingCommandHandler(ICourseOfferingService service)
    : IRequestHandler<CreateCourseOfferingCommand, CourseOfferingDto>
{
    public Task<CourseOfferingDto> Handle(CreateCourseOfferingCommand request, CancellationToken cancellationToken)
        => service.CreateOfferingAsync(request.AcademicTermId, request.CourseId, request.Capacity, cancellationToken);
}
