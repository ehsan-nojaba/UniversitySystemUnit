using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر ProfessorAvailability؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class ProfessorAvailabilityLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static ProfessorAvailability Create(long professorTeachingRequestId, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime, long? courseId = null)
    {
        var entity = new ProfessorAvailability();
        entity.ProfessorTeachingRequestId = professorTeachingRequestId;
        entity.CourseId = courseId;
        entity.DayOfWeek = dayOfWeek;
        entity.StartTime = startTime;
        entity.EndTime = endTime;
        return entity;
    }
}
