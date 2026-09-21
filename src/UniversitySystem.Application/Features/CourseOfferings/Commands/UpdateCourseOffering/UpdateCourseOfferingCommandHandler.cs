using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Services;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;

/// <summary>
/// درخواست «ویرایش ظرفیت و وضعیت ارائه» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class UpdateCourseOfferingCommandHandler(ICourseOfferingService service)
    : IRequestHandler<UpdateCourseOfferingCommand, CourseOfferingDto>
{
    public Task<CourseOfferingDto> Handle(UpdateCourseOfferingCommand request, CancellationToken cancellationToken)
    {
        return service.UpdateOfferingAsync(request.Id, request.Capacity, request.IsActive, cancellationToken);
    }
}
