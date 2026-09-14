using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// موجودیت پیش‌نیاز درس: تعریف‌کننده وابستگی پیش‌نیازی میان یک درس و درس پیش‌نیاز آن.
/// </summary>
public class CoursePrerequisite : BaseAuditableEntity
{
    public long CourseId { get; private set; }
    public long PrerequisiteCourseId { get; private set; }

    public Course Course { get; private set; } = default!;
    public Course PrerequisiteCourse { get; private set; } = default!;

    private CoursePrerequisite() { }

    internal CoursePrerequisite(long courseId, long prerequisiteCourseId)
    {
        CourseId = courseId;
        PrerequisiteCourseId = prerequisiteCourseId;
    }
}
