using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// چارت یک رشته با نسخه مشخص؛ درس‌های پیشنهادی و الزامی رشته را در خود جمع می‌کند.
/// </summary>
public class Curriculum : BaseAuditableEntity
{
    public long MajorId { get; private set; }
    public string Title { get; private set; } = default!;
    public string Version { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public Major Major { get; private set; } = default!;

    private readonly List<CurriculumCourse> _curriculumCourses = new();
    public IReadOnlyCollection<CurriculumCourse> CurriculumCourses => _curriculumCourses.AsReadOnly();

    private Curriculum()
    {
    }

    public Curriculum(long majorId, string title, string version)
    {
        MajorId = majorId;
        Title = title.Trim();
        Version = version.Trim();
        IsActive = true;
    }

    public void AddCourse(long courseId, int recommendedTerm, bool isRequired)
    {
        bool alreadyAdded = _curriculumCourses.Any(cc => cc.CourseId == courseId);
        if (!alreadyAdded)
        {
            _curriculumCourses.Add(new CurriculumCourse(Id, courseId, recommendedTerm, isRequired));
        }
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
