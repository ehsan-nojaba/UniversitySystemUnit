using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// تعریف پایه درس شامل کد، عنوان و تعداد واحد؛ مستقل از ترم، استاد و ظرفیت کلاس است.
/// </summary>
public class Course : BaseAuditableEntity
{
    public string Code { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public int Credits { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<CoursePrerequisite> _prerequisites = new();
    public IReadOnlyCollection<CoursePrerequisite> Prerequisites => _prerequisites.AsReadOnly();

    private readonly List<CourseOffering> _offerings = new();
    public IReadOnlyCollection<CourseOffering> Offerings => _offerings.AsReadOnly();

    private Course() { }

    public Course(string code, string title, int credits)
    {
        Code = code.Trim();
        Title = title.Trim();
        Credits = credits;
        IsActive = true;
    }

    public void AddPrerequisite(long prerequisiteCourseId)
    {
        bool alreadyExists = _prerequisites.Any(p => p.PrerequisiteCourseId == prerequisiteCourseId);
        if (!alreadyExists)
            _prerequisites.Add(new CoursePrerequisite(Id, prerequisiteCourseId));
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
