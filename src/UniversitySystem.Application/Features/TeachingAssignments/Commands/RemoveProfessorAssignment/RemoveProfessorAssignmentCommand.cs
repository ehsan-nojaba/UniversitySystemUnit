using MediatR;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;

public class RemoveProfessorAssignmentCommand : IRequest<Unit>
{
    public long CourseOfferingId { get; set; }
    public long ProfessorId { get; set; }
}
