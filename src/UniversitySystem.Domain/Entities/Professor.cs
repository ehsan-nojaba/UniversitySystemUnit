using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

public class Professor : BaseAuditableEntity
{
    public long UserId { get; private set; }
    public string PersonnelCode { get; private set; } = default!;

    public User User { get; private set; } = default!;

    private readonly List<ProfessorTeachingRequest> _teachingRequests = new();
    public IReadOnlyCollection<ProfessorTeachingRequest> TeachingRequests => _teachingRequests.AsReadOnly();

    private readonly List<TeachingAssignment> _teachingAssignments = new();
    public IReadOnlyCollection<TeachingAssignment> TeachingAssignments => _teachingAssignments.AsReadOnly();

    private Professor() { }

    public Professor(long userId, string personnelCode)
    {
        UserId = userId;
        PersonnelCode = personnelCode.Trim();
    }
}
