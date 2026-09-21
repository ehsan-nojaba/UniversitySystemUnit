using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Services;

namespace UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;

/// <summary>
/// درخواست «دریافت ارائه‌های یک ترم» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetCourseOfferingsQueryHandler(ICourseOfferingService service)
    : IRequestHandler<GetCourseOfferingsQuery, ICollection<CourseOfferingDto>>
{
    public Task<ICollection<CourseOfferingDto>> Handle(GetCourseOfferingsQuery request, CancellationToken cancellationToken)
    {
        return service.GetOfferingsByTermAsync(request.AcademicTermId, cancellationToken);
    }
}
