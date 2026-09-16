using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// دانشکده؛ بالاترین سطح ساختار آموزشی این پروژه و محل گروه‌های آموزشی است.
/// </summary>
public class Faculty : BaseAuditableEntity
{
    public string Code { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; }

    private readonly List<Department> _departments = new();
    public IReadOnlyCollection<Department> Departments => _departments.AsReadOnly();

    private Faculty()
    {
    }

    public Faculty(string code, string title)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Faculty code cannot be empty.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Faculty title cannot be empty.", nameof(title));
        }

        Code = code.Trim();
        Title = title.Trim();
        IsActive = true;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
