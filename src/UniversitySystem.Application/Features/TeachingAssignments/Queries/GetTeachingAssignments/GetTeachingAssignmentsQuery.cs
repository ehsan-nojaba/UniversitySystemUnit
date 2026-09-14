using MediatR;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;

namespace UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;

public class GetTeachingAssignmentsQuery : IRequest<ICollection<TeachingAssignmentDto>>
{
    public long CourseOfferingId { get; set; }
}
