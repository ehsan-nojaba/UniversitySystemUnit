using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Services;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;

public sealed class UpdateCourseOfferingCommandHandler(ICourseOfferingService service)
    : IRequestHandler<UpdateCourseOfferingCommand, CourseOfferingDto>
{
    public Task<CourseOfferingDto> Handle(UpdateCourseOfferingCommand request, CancellationToken cancellationToken)
        => service.UpdateOfferingAsync(request.Id, request.Capacity, request.IsActive, cancellationToken);
}
