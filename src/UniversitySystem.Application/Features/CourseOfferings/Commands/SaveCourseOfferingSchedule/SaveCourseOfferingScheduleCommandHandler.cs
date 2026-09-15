using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Services;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.SaveCourseOfferingSchedule;

public sealed class SaveCourseOfferingScheduleCommandHandler(ICourseOfferingService service)
    : IRequestHandler<SaveCourseOfferingScheduleCommand, ICollection<CourseOfferingScheduleDto>>
{
    public Task<ICollection<CourseOfferingScheduleDto>> Handle(SaveCourseOfferingScheduleCommand request, CancellationToken cancellationToken)
        => service.SaveScheduleAsync(request.CourseOfferingId, request.Slots, cancellationToken);
}
