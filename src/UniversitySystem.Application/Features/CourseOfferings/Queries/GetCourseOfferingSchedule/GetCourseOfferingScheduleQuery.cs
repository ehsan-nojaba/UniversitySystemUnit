using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferingSchedule;

/// <summary>
/// درخواست خواندن اطلاعات برای «مشاهده زمان‌بندی ارائه»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public class GetCourseOfferingScheduleQuery : IRequest<ICollection<CourseOfferingScheduleDto>>
{
    public long CourseOfferingId { get; set; }
}
