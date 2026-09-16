using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// تخصیص نهایی یک استاد به ارائه درس، همراه زمان تخصیص؛ مستقل از اعلام علاقه استاد است.
/// </summary>
public class TeachingAssignment : BaseAuditableEntity
{
    public long CourseOfferingId { get; private set; }
    public long ProfessorId { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public CourseOffering CourseOffering { get; private set; } = default!;
    public Professor Professor { get; private set; } = default!;

    private TeachingAssignment()
    {
    }

    public TeachingAssignment(long courseOfferingId, long professorId, DateTime assignedAt)
    {
        if (courseOfferingId <= 0)
        {
            throw new ArgumentException("TeachingAssignment must have a valid CourseOffering.", nameof(courseOfferingId));
        }

        if (professorId <= 0)
        {
            throw new ArgumentException("TeachingAssignment must have a valid Professor.", nameof(professorId));
        }

        CourseOfferingId = courseOfferingId;
        ProfessorId = professorId;
        AssignedAt = assignedAt;
    }
}
