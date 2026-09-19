using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// گروه آموزشی زیرمجموعه دانشکده؛ رشته‌های مرتبط را دسته‌بندی می‌کند.
/// </summary>
public class Department : BaseAuditableEntity
{
    public long FacultyId { get; set; }
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public bool IsActive { get; set; }
    public Faculty Faculty { get; set; } = default!;
    public ICollection<Major> Majors { get; set; } = new List<Major>();
}
