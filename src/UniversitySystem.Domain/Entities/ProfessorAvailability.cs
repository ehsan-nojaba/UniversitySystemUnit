using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// بازه زمانی آزاد استاد در یک روز هفته، متعلق به درخواست تدریس؛ با زمان‌بندی نهایی کلاس تفاوت دارد.
/// </summary>
public class ProfessorAvailability : BaseAuditableEntity
{
    public long ProfessorTeachingRequestId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public ProfessorTeachingRequest ProfessorTeachingRequest { get; private set; } = default!;

    private ProfessorAvailability() { }

    internal ProfessorAvailability(long professorTeachingRequestId, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        ProfessorTeachingRequestId = professorTeachingRequestId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }
}
