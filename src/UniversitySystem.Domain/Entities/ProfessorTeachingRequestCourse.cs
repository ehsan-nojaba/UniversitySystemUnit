using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// یک درس پیشنهادی استاد در درخواست تدریس؛ علاقه و اولویت استاد را ثبت می‌کند و به معنی تخصیص نهایی نیست.
/// </summary>
public class ProfessorTeachingRequestCourse : BaseAuditableEntity
{
    public long ProfessorTeachingRequestId { get; private set; }
    public long CourseId { get; private set; }
    public int Priority { get; private set; }
    public ProfessorTeachingRequest ProfessorTeachingRequest { get; private set; } = default!;
    public Course Course { get; private set; } = default!;

    internal void UpdatePriority(int priority)
    {
        if (priority <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(priority));
        }

        Priority = priority;
    }

    private ProfessorTeachingRequestCourse()
    {
    }

    internal ProfessorTeachingRequestCourse(long professorTeachingRequestId, long courseId, int priority)
    {
        ProfessorTeachingRequestId = professorTeachingRequestId;
        CourseId = courseId;
        Priority = priority;
    }
}
