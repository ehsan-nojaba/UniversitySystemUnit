using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// یک درس پیشنهادی استاد در درخواست تدریس؛ علاقه و اولویت استاد را ثبت می‌کند و به معنی تخصیص نهایی نیست.
/// </summary>
public class ProfessorTeachingRequestCourse : BaseAuditableEntity
{
    public long ProfessorTeachingRequestId { get; set; }
    public long CourseId { get; set; }
    public int Priority { get; set; }
    public ProfessorTeachingRequest ProfessorTeachingRequest { get; set; } = default!;
    public Course Course { get; set; } = default!;
}
