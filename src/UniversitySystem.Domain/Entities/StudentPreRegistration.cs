using UniversitySystem.Domain.Common;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// درخواست پیش‌انتخاب دانشجو برای یک ترم؛ درس‌های موردنیاز و وضعیت پیش‌نویس یا ارسال‌شده را مدیریت می‌کند.
/// </summary>
public class StudentPreRegistration : BaseAuditableEntity
{
    public long StudentId { get; private set; }
    public long AcademicTermId { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateTime? SubmittedAt { get; private set; }

    public Student Student { get; private set; } = default!;
    public AcademicTerm AcademicTerm { get; private set; } = default!;

    private readonly List<StudentPreRegistrationItem> _items = new();
    public IReadOnlyCollection<StudentPreRegistrationItem> Items => _items.AsReadOnly();

    private StudentPreRegistration() { }

    public StudentPreRegistration(long studentId, long academicTermId)
    {
        StudentId = studentId;
        AcademicTermId = academicTermId;
        Status = RequestStatus.Draft;
    }

    public void AddCourse(long courseId, int priority)
    {
        EnsureEditable();

        bool alreadyAdded = _items.Any(i => i.CourseId == courseId);
        if (!alreadyAdded)
            _items.Add(new StudentPreRegistrationItem(Id, courseId, priority));
    }

    public void RemoveCourse(long courseId)
    {
        EnsureEditable();

        var item = _items.FirstOrDefault(i => i.CourseId == courseId);
        if (item is not null)
            _items.Remove(item);
    }

    public void UpdateCoursePriority(long courseId, int priority)
    {
        EnsureEditable();

        var item = _items.FirstOrDefault(i => i.CourseId == courseId);
        if (item is not null)
            item.UpdatePriority(priority);
    }

    public void Submit(DateTime utcNow)
    {
        EnsureEditable();

        if (_items.Count == 0)
            throw new InvalidOperationException("Cannot submit a pre-registration with no courses selected.");

        Status = RequestStatus.Submitted;
        SubmittedAt = utcNow;
    }

    public void Cancel()
    {
        if (Status == RequestStatus.Cancelled)
            return;

        Status = RequestStatus.Cancelled;
    }

    private void EnsureEditable()
    {
        if (Status != RequestStatus.Draft)
            throw new InvalidOperationException("Pre-registration can only be modified while in Draft status.");
    }
}
