using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Services;

namespace UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferingSchedule;

public sealed class GetCourseOfferingScheduleQueryHandler(ICourseOfferingService service)
    : IRequestHandler<GetCourseOfferingScheduleQuery, ICollection<CourseOfferingScheduleDto>>
{
    public Task<ICollection<CourseOfferingScheduleDto>> Handle(GetCourseOfferingScheduleQuery request, CancellationToken cancellationToken)
        => service.GetScheduleAsync(request.CourseOfferingId, cancellationToken);
}
