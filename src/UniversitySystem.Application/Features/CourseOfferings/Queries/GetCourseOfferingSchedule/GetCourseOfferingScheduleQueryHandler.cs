using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Services;

namespace UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferingSchedule;

/// <summary>
/// درخواست «مشاهده زمان‌بندی ارائه» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetCourseOfferingScheduleQueryHandler(ICourseOfferingService service)
    : IRequestHandler<GetCourseOfferingScheduleQuery, ICollection<CourseOfferingScheduleDto>>
{
    public Task<ICollection<CourseOfferingScheduleDto>> Handle(GetCourseOfferingScheduleQuery request, CancellationToken cancellationToken)
        => service.GetScheduleAsync(request.CourseOfferingId, cancellationToken);
}
