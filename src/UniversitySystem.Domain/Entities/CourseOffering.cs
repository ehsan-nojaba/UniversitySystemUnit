using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// ارائه واقعی یک درس در یک ترم، همراه ظرفیت و وضعیت فعالیت؛ تخصیص استاد و زمان کلاس به آن متصل می‌شوند.
/// </summary>
public class CourseOffering : BaseAuditableEntity
{
    public long CourseId { get; private set; }
    public long AcademicTermId { get; private set; }
    public int Capacity { get; private set; }
    public bool IsActive { get; private set; }
    public Course Course { get; private set; } = default!;
    public AcademicTerm AcademicTerm { get; private set; } = default!;

    private readonly List<TeachingAssignment> _teachingAssignments = new();
    public IReadOnlyCollection<TeachingAssignment> TeachingAssignments => _teachingAssignments.AsReadOnly();

    private readonly List<Enrollment> _enrollments = new();
    public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();

    private readonly List<CourseOfferingSchedule> _schedules = new();
    public IReadOnlyCollection<CourseOfferingSchedule> Schedules => _schedules.AsReadOnly();

    private CourseOffering()
    {
    }

    public CourseOffering(long courseId, long academicTermId, int capacity)
    {
        if (courseId <= 0)
        {
            throw new ArgumentException("CourseOffering must have a valid Course.", nameof(courseId));
        }

        if (academicTermId <= 0)
        {
            throw new ArgumentException("CourseOffering must have a valid AcademicTerm.", nameof(academicTermId));
        }

        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be a positive number.", nameof(capacity));
        }

        CourseId = courseId;
        AcademicTermId = academicTermId;
        Capacity = capacity;
        IsActive = true;
    }

    public void UpdateCapacity(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be a positive number.", nameof(capacity));
        }

        Capacity = capacity;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
