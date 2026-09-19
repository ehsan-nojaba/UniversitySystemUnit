using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// تعریف پایه درس شامل کد، عنوان و تعداد واحد؛ مستقل از ترم، استاد و ظرفیت کلاس است.
/// </summary>
public class Course : BaseAuditableEntity
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int Credits { get; set; }
    public bool IsActive { get; set; }
    public ICollection<CoursePrerequisite> Prerequisites { get; set; } = new List<CoursePrerequisite>();
    public ICollection<CourseOffering> Offerings { get; set; } = new List<CourseOffering>();
}
