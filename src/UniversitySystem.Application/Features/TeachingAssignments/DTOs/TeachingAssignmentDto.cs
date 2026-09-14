namespace UniversitySystem.Application.Features.TeachingAssignments.DTOs;

public class TeachingAssignmentDto
{
    public long TeachingAssignmentId { get; set; }
    public long ProfessorId { get; set; }
    public string ProfessorFullName { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
    public bool RequestedThisCourse { get; set; }
    public int? RequestedPriority { get; set; }
}
