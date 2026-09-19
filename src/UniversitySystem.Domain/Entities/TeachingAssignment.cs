using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// تخصیص نهایی یک استاد به ارائه درس، همراه زمان تخصیص؛ مستقل از اعلام علاقه استاد است.
/// </summary>
public class TeachingAssignment : BaseAuditableEntity
{
    public long CourseOfferingId { get; set; }
    public long ProfessorId { get; set; }
    public DateTime AssignedAt { get; set; }
    public CourseOffering CourseOffering { get; set; } = default!;
    public Professor Professor { get; set; } = default!;
}
