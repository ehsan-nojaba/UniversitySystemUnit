using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// روز و بازه زمانی برگزاری یک ارائه درس؛ برای نمایش برنامه و بررسی تداخل استفاده می‌شود.
/// </summary>
public class CourseOfferingSchedule : BaseAuditableEntity
{
    public long CourseOfferingId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public CourseOffering CourseOffering { get; set; } = default!;
}
