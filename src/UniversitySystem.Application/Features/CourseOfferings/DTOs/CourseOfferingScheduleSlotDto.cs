namespace UniversitySystem.Application.Features.CourseOfferings.DTOs;

/// <summary>
/// اسلات زمانی برای ثبت یا به‌روزرسانی زمان‌بندی ارائه درس.
/// </summary>
public class CourseOfferingScheduleSlotDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
