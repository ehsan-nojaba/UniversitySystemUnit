namespace UniversitySystem.Application.Features.TeachingAssignments.DTOs;

/// <summary>
/// پاسخ تخصیص استاد: شناسه تخصیص، نام استاد، زمان تخصیص و اینکه استاد قبلاً این درس را درخواست کرده است یا خیر.
/// </summary>
public class TeachingAssignmentDto
{
    public long TeachingAssignmentId { get; set; }
    public long ProfessorId { get; set; }
    public string ProfessorFullName { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
    public bool RequestedThisCourse { get; set; }
    public int? RequestedPriority { get; set; }
}
