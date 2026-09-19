using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// رابط میان درس و پیش‌نیاز آن؛ برای بررسی مجاز بودن انتخاب درس استفاده می‌شود.
/// </summary>
public class CoursePrerequisite : BaseAuditableEntity
{
    public long CourseId { get; set; }
    public long PrerequisiteCourseId { get; set; }
    public Course Course { get; set; } = default!;
    public Course PrerequisiteCourse { get; set; } = default!;
}
