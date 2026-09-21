using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Services;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.SaveCourseOfferingSchedule;

/// <summary>
/// درخواست «ذخیره زمان‌بندی ارائه با کنترل تداخل» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class SaveCourseOfferingScheduleCommandHandler(ICourseOfferingService service)
    : IRequestHandler<SaveCourseOfferingScheduleCommand, ICollection<CourseOfferingScheduleDto>>
{
    public Task<ICollection<CourseOfferingScheduleDto>> Handle(SaveCourseOfferingScheduleCommand request, CancellationToken cancellationToken)
    {
        return service.SaveScheduleAsync(request.CourseOfferingId, request.Slots, cancellationToken);
    }
}
