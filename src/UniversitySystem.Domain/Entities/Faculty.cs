using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// دانشکده؛ بالاترین سطح ساختار آموزشی این پروژه و محل گروه‌های آموزشی است.
/// </summary>
public class Faculty : BaseAuditableEntity
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public bool IsActive { get; set; }
    public ICollection<Department> Departments { get; set; } = new List<Department>();
}
