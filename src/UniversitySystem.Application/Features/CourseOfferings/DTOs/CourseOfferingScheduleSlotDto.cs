namespace UniversitySystem.Application.Features.CourseOfferings.DTOs;

/// <summary>
/// ورودی روز و زمان کلاس برای ذخیره برنامه؛ با زمان آزاد پیشنهادی استاد متفاوت است.
/// </summary>
public class CourseOfferingScheduleSlotDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
