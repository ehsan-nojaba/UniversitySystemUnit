using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// چارت یک رشته با نسخه مشخص؛ درس‌های پیشنهادی و الزامی رشته را در خود جمع می‌کند.
/// </summary>
public class Curriculum : BaseAuditableEntity
{
    public long MajorId { get; set; }
    public string Title { get; set; } = default!;
    public string Version { get; set; } = default!;
    public bool IsActive { get; set; }
    public Major Major { get; set; } = default!;
    public ICollection<CurriculumCourse> CurriculumCourses { get; set; } = new List<CurriculumCourse>();
}
