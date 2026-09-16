using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;

/// <summary>
/// درخواست خواندن اطلاعات برای «دریافت ارائه‌های یک ترم»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public class GetCourseOfferingsQuery : IRequest<ICollection<CourseOfferingDto>>
{
    public long AcademicTermId { get; set; }
}
