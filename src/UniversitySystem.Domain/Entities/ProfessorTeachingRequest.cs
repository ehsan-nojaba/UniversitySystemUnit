using UniversitySystem.Domain.Common;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// درخواست تدریس استاد برای یک ترم؛ درس‌ها، اولویت‌ها و زمان‌های آزاد را جمع می‌کند و پس از ارسال قابل ویرایش نیست.
/// </summary>
public class ProfessorTeachingRequest : BaseAuditableEntity
{
    public long ProfessorId { get; set; }
    public long AcademicTermId { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public Professor Professor { get; set; } = default!;
    public AcademicTerm AcademicTerm { get; set; } = default!;
    public ICollection<ProfessorTeachingRequestCourse> Courses { get; set; } = new List<ProfessorTeachingRequestCourse>();
    public ICollection<ProfessorAvailability> Availabilities { get; set; } = new List<ProfessorAvailability>();
}
