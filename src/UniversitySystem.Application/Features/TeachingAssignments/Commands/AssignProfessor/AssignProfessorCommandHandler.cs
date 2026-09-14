using MediatR;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Services;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;

public sealed class AssignProfessorCommandHandler(ITeachingAssignmentService service)
    : IRequestHandler<AssignProfessorCommand, TeachingAssignmentDto>
{
    public Task<TeachingAssignmentDto> Handle(AssignProfessorCommand request, CancellationToken cancellationToken)
        => service.AssignProfessorAsync(request.CourseOfferingId, request.ProfessorId, cancellationToken);
}
