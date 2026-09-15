using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.SaveCourseOfferingSchedule;

public class SaveCourseOfferingScheduleCommand : IRequest<ICollection<CourseOfferingScheduleDto>>
{
    public long CourseOfferingId { get; set; }
    public List<CourseOfferingScheduleSlotDto> Slots { get; set; } = [];
}
