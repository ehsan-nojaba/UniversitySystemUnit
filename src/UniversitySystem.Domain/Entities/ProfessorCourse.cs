using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>ارتباط پایه استاد با درس‌هایی که آموزش اجازه تدریس آن‌ها را داده است؛ با تخصیص نهایی به کلاس فرق دارد.</summary>
public sealed class ProfessorCourse : BaseAuditableEntity
{
    public long ProfessorId { get; private set; }
    public long CourseId { get; private set; }
    public Professor Professor { get; private set; } = default!;
    public Course Course { get; private set; } = default!;
    private ProfessorCourse() { }
    public ProfessorCourse(long professorId, long courseId)
    {
        if (professorId <= 0 || courseId <= 0) { throw new ArgumentOutOfRangeException(nameof(courseId)); }
        ProfessorId = professorId;
        CourseId = courseId;
    }
}
