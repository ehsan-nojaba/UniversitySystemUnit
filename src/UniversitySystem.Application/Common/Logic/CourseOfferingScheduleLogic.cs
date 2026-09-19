using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر CourseOfferingSchedule؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class CourseOfferingScheduleLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static CourseOfferingSchedule Create(long courseOfferingId, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        var entity = new CourseOfferingSchedule();
        if (courseOfferingId <= 0)
        {
            throw new ArgumentException("CourseOfferingSchedule must have a valid CourseOffering.", nameof(courseOfferingId));
        }

        if (!Enum.IsDefined(dayOfWeek))
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "Invalid DayOfWeek value.");
        }

        if (endTime <= startTime)
        {
            throw new ArgumentException("EndTime must be after StartTime.", nameof(endTime));
        }

        entity.CourseOfferingId = courseOfferingId;
        entity.DayOfWeek = dayOfWeek;
        entity.StartTime = startTime;
        entity.EndTime = endTime;
        return entity;
    }
}
