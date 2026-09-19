using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// پروفایل استاد؛ حساب کاربری را به کد استادی، درخواست‌های تدریس و تخصیص‌های تدریس متصل می‌کند.
/// </summary>
public class Professor : BaseAuditableEntity
{
    public long UserId { get; set; }
    public string PersonnelCode { get; set; } = default!;
    public User User { get; set; } = default!;
    public ICollection<ProfessorTeachingRequest> TeachingRequests { get; set; } = new List<ProfessorTeachingRequest>();
    public ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();
}
