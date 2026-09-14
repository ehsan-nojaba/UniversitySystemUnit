using MediatR;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;

public class AssignProfessorCommand : IRequest<TeachingAssignmentDto>
{
    public long CourseOfferingId { get; set; }
    public long ProfessorId { get; set; }
}
