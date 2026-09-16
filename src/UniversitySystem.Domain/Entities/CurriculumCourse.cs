using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// عضویت یک درس در چارت؛ ترم پیشنهادی و الزامی بودن درس را نگه می‌دارد.
/// </summary>
public class CurriculumCourse : BaseAuditableEntity
{
    public long CurriculumId { get; private set; }
    public long CourseId { get; private set; }
    public int RecommendedTerm { get; private set; }
    public bool IsRequired { get; private set; }

    public Curriculum Curriculum { get; private set; } = default!;
    public Course Course { get; private set; } = default!;

    private CurriculumCourse() { }

    internal CurriculumCourse(long curriculumId, long courseId, int recommendedTerm, bool isRequired)
    {
        CurriculumId = curriculumId;
        CourseId = courseId;
        RecommendedTerm = recommendedTerm;
        IsRequired = isRequired;
    }
}
