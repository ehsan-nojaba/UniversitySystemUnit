using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// عضویت یک درس در چارت؛ ترم پیشنهادی و الزامی بودن درس را نگه می‌دارد.
/// </summary>
public class CurriculumCourse : BaseAuditableEntity
{
    public long CurriculumId { get; set; }
    public long CourseId { get; set; }
    public int RecommendedTerm { get; set; }
    public bool IsRequired { get; set; }
    public Curriculum Curriculum { get; set; } = default!;
    public Course Course { get; set; } = default!;
}
