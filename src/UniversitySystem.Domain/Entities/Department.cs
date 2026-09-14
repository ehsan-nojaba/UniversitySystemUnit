using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// موجودیت گروه آموزشی: دپارتمان یا گروه آموزشی تخصصی زیرمجموعه یک دانشکده.
/// </summary>
public class Department : BaseAuditableEntity
{
    public long FacultyId { get; private set; }
    public string Code { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; }

    public Faculty Faculty { get; private set; } = default!;

    private readonly List<Major> _majors = new();
    public IReadOnlyCollection<Major> Majors => _majors.AsReadOnly();

    private Department() { }

    public Department(long facultyId, string code, string title)
    {
        if (facultyId <= 0)
            throw new ArgumentException("Department must have a valid Faculty.", nameof(facultyId));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Department code cannot be empty.", nameof(code));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Department title cannot be empty.", nameof(title));

        FacultyId = facultyId;
        Code = code.Trim();
        Title = title.Trim();
        IsActive = true;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
