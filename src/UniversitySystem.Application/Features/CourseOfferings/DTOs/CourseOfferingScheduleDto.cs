namespace UniversitySystem.Application.Features.CourseOfferings.DTOs;

/// <summary>
/// مدل انتقال داده برای بازخوانی زمان‌بندی ارائه درس.
/// </summary>
public class CourseOfferingScheduleDto
{
    public long Id { get; set; }
    public long CourseOfferingId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
