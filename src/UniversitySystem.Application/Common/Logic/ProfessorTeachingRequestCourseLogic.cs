using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر ProfessorTeachingRequestCourse؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class ProfessorTeachingRequestCourseLogic
{
    public static void UpdatePriority(ProfessorTeachingRequestCourse entity, int priority)
    {
        if (priority <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(priority));
        }

        entity.Priority = priority;
    }

    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static ProfessorTeachingRequestCourse Create(long professorTeachingRequestId, long courseId, int priority)
    {
        var entity = new ProfessorTeachingRequestCourse();
        entity.ProfessorTeachingRequestId = professorTeachingRequestId;
        entity.CourseId = courseId;
        entity.Priority = priority;
        return entity;
    }
}
