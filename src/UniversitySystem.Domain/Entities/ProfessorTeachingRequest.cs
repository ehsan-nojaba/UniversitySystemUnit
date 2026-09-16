using UniversitySystem.Domain.Common;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// درخواست تدریس استاد برای یک ترم؛ درس‌ها، اولویت‌ها و زمان‌های آزاد را جمع می‌کند و پس از ارسال قابل ویرایش نیست.
/// </summary>
public class ProfessorTeachingRequest : BaseAuditableEntity
{
    public long ProfessorId { get; private set; }
    public long AcademicTermId { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateTime? SubmittedAt { get; private set; }
    public Professor Professor { get; private set; } = default!;
    public AcademicTerm AcademicTerm { get; private set; } = default!;

    private readonly List<ProfessorTeachingRequestCourse> _courses = new();
    public IReadOnlyCollection<ProfessorTeachingRequestCourse> Courses => _courses.AsReadOnly();

    private readonly List<ProfessorAvailability> _availabilities = new();
    public IReadOnlyCollection<ProfessorAvailability> Availabilities => _availabilities.AsReadOnly();

    private ProfessorTeachingRequest()
    {
    }

    public ProfessorTeachingRequest(long professorId, long academicTermId)
    {
        ProfessorId = professorId;
        AcademicTermId = academicTermId;
        Status = RequestStatus.Draft;
    }

    public void AddCourse(long courseId, int priority)
    {
        EnsureEditable();
        if (courseId <= 0 || priority <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(courseId));
        }

        bool alreadyAdded = _courses.Any(c => c.CourseId == courseId);
        if (!alreadyAdded)
        {
            _courses.Add(new ProfessorTeachingRequestCourse(Id, courseId, priority));
        }
    }

    public void UpdateCoursePriority(long courseId, int priority)
    {
        EnsureEditable();
        var course = _courses.Single(c => c.CourseId == courseId);
        course.UpdatePriority(priority);
    }

    public void ReplaceAvailability(IEnumerable<(DayOfWeek Day, TimeOnly Start, TimeOnly End)> intervals)
    {
        EnsureEditable();
        var values = intervals.ToList();
        if (values.Any(v => !Enum.IsDefined(v.Day) || v.Start >= v.End) || values.Select((v, i) => values.Skip(i + 1).Any(n => v.Day == n.Day && v.Start < n.End && n.Start < v.End)).Any(x => x))
        {
            throw new InvalidOperationException("Availability intervals must be valid and cannot overlap.");
        }

        _availabilities.Clear();
        foreach (var value in values)
            AddAvailability(value.Day, value.Start, value.End);
    }

    public void RemoveCourse(long courseId)
    {
        EnsureEditable();
        var course = _courses.FirstOrDefault(c => c.CourseId == courseId);
        if (course is not null)
        {
            _courses.Remove(course);
        }
    }

    public void AddAvailability(DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        EnsureEditable();
        if (!Enum.IsDefined(dayOfWeek))
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
        }

        if (endTime <= startTime)
        {
            throw new InvalidOperationException("EndTime must be after StartTime.");
        }

        if (_availabilities.Any(a => a.DayOfWeek == dayOfWeek && a.StartTime < endTime && startTime < a.EndTime))
        {
            throw new InvalidOperationException("Availability intervals cannot overlap.");
        }

        bool alreadyExists = _availabilities.Any(a => a.DayOfWeek == dayOfWeek && a.StartTime == startTime && a.EndTime == endTime);
        if (!alreadyExists)
        {
            _availabilities.Add(new ProfessorAvailability(Id, dayOfWeek, startTime, endTime));
        }
    }

    public void RemoveAvailability(long availabilityId)
    {
        EnsureEditable();
        var availability = _availabilities.FirstOrDefault(a => a.Id == availabilityId);
        if (availability is not null)
        {
            _availabilities.Remove(availability);
        }
    }

    public void Submit(DateTime utcNow)
    {
        EnsureEditable();
        if (_courses.Count == 0)
        {
            throw new InvalidOperationException("Cannot submit a teaching request with no courses selected.");
        }

        Status = RequestStatus.Submitted;
        SubmittedAt = utcNow;
    }

    public void Cancel()
    {
        if (Status == RequestStatus.Cancelled)
        {
            return;
        }

        Status = RequestStatus.Cancelled;
    }

    private void EnsureEditable()
    {
        if (Status != RequestStatus.Draft)
        {
            throw new InvalidOperationException("Teaching request can only be modified while in Draft status.");
        }
    }
}
