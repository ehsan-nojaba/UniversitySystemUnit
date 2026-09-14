using MediatR;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Services;

namespace UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;

public sealed class GetTeachingAssignmentsQueryHandler(ITeachingAssignmentService service)
    : IRequestHandler<GetTeachingAssignmentsQuery, ICollection<TeachingAssignmentDto>>
{
    public Task<ICollection<TeachingAssignmentDto>> Handle(GetTeachingAssignmentsQuery request, CancellationToken cancellationToken)
        => service.GetAssignmentsAsync(request.CourseOfferingId, cancellationToken);
}
