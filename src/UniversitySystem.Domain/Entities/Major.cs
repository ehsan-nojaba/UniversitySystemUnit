using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// رشته تحصیلی دانشجو؛ برای پیدا کردن چارت درسی مناسب استفاده می‌شود.
/// </summary>
public class Major : BaseAuditableEntity
{
    public long DepartmentId { get; set; }
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public bool IsActive { get; set; }
    public Department Department { get; set; } = default!;
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Curriculum> Curriculums { get; set; } = new List<Curriculum>();
}
