using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// موجودیت درس انتخابی تدریس: درسی که استاد تمایل به تدریس آن در ترم موردنظر دارد به همراه اولویت.
/// </summary>
public class ProfessorTeachingRequestCourse : BaseAuditableEntity
{
    public long ProfessorTeachingRequestId { get; private set; }
    public long CourseId { get; private set; }
    public int Priority { get; private set; }

    public ProfessorTeachingRequest ProfessorTeachingRequest { get; private set; } = default!;
    public Course Course { get; private set; } = default!;

    private ProfessorTeachingRequestCourse() { }

    internal ProfessorTeachingRequestCourse(long professorTeachingRequestId, long courseId, int priority)
    {
        ProfessorTeachingRequestId = professorTeachingRequestId;
        CourseId = courseId;
        Priority = priority;
    }
}
