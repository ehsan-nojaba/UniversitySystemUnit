using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر StudentPreRegistrationItem؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class StudentPreRegistrationItemLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static StudentPreRegistrationItem Create(long studentPreRegistrationId, long courseId, int priority)
    {
        var entity = new StudentPreRegistrationItem();
        entity.StudentPreRegistrationId = studentPreRegistrationId;
        entity.CourseId = courseId;
        entity.Priority = priority;
        return entity;
    }

    public static void UpdatePriority(StudentPreRegistrationItem entity, int priority)
    {
        entity.Priority = priority;
    }
}
