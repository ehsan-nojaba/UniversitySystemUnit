using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.SaveCourseOfferingSchedule;

/// <summary>
/// درخواست انجام عملیات «ذخیره زمان‌بندی ارائه با کنترل تداخل»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public class SaveCourseOfferingScheduleCommand : IRequest<ICollection<CourseOfferingScheduleDto>>
{
    public long CourseOfferingId { get; set; }
    public List<CourseOfferingScheduleSlotDto> Slots { get; set; } = [];
}
