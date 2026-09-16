using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// روز و بازه زمانی برگزاری یک ارائه درس؛ برای نمایش برنامه و بررسی تداخل استفاده می‌شود.
/// </summary>
public class CourseOfferingSchedule : BaseAuditableEntity
{
    public long CourseOfferingId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public CourseOffering CourseOffering { get; private set; } = default!;

    private CourseOfferingSchedule() { }

    public CourseOfferingSchedule(long courseOfferingId, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        if (courseOfferingId <= 0)
            throw new ArgumentException("CourseOfferingSchedule must have a valid CourseOffering.", nameof(courseOfferingId));

        if (!Enum.IsDefined(dayOfWeek))
            throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "Invalid DayOfWeek value.");

        if (endTime <= startTime)
            throw new ArgumentException("EndTime must be after StartTime.", nameof(endTime));

        CourseOfferingId = courseOfferingId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }
}
