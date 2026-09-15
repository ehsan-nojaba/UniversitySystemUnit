using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferingSchedule;

public class GetCourseOfferingScheduleQuery : IRequest<ICollection<CourseOfferingScheduleDto>>
{
    public long CourseOfferingId { get; set; }
}
