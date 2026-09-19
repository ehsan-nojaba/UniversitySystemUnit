using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// بازه زمانی آزاد استاد در یک روز هفته، متعلق به درخواست تدریس؛ با زمان‌بندی نهایی کلاس تفاوت دارد.
/// </summary>
public class ProfessorAvailability : BaseAuditableEntity
{
    public long ProfessorTeachingRequestId { get; set; }
    /// <summary>درس مربوط به این پیشنهاد زمانی؛ مقدار خالی فقط برای سوابق قدیمی است.</summary>
    public long? CourseId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public ProfessorTeachingRequest ProfessorTeachingRequest { get; set; } = default!;
}
