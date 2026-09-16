using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// رشته تحصیلی دانشجو؛ برای پیدا کردن چارت درسی مناسب استفاده می‌شود.
/// </summary>
public class Major : BaseAuditableEntity
{
    public long DepartmentId { get; private set; }
    public string Code { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public Department Department { get; private set; } = default!;

    private readonly List<Student> _students = new();
    public IReadOnlyCollection<Student> Students => _students.AsReadOnly();

    private readonly List<Curriculum> _curriculums = new();
    public IReadOnlyCollection<Curriculum> Curriculums => _curriculums.AsReadOnly();

    private Major()
    {
    }

    public Major(long departmentId, string code, string title)
    {
        if (departmentId <= 0)
        {
            throw new ArgumentException("Major must have a valid Department.", nameof(departmentId));
        }

        DepartmentId = departmentId;
        Code = code.Trim();
        Title = title.Trim();
        IsActive = true;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
