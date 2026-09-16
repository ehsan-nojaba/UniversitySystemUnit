namespace UniversitySystem.Application.Features.CourseOfferings.DTOs;

/// <summary>
/// پاسخ یک بازه ثبت‌شده کلاس شامل شناسه بازه و ارائه، روز هفته و زمان شروع و پایان.
/// </summary>
public class CourseOfferingScheduleDto
{
    public long Id { get; set; }
    public long CourseOfferingId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
