using MediatR;
using UniversitySystem.Application.Features.TeachingAssignments.Services;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;

public sealed class RemoveProfessorAssignmentCommandHandler(ITeachingAssignmentService service)
    : IRequestHandler<RemoveProfessorAssignmentCommand, Unit>
{
    public async Task<Unit> Handle(RemoveProfessorAssignmentCommand request, CancellationToken cancellationToken)
    {
        await service.RemoveAssignmentAsync(request.CourseOfferingId, request.ProfessorId, cancellationToken);
        return Unit.Value;
    }
}
