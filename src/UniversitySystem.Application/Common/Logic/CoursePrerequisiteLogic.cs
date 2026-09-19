using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر CoursePrerequisite؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class CoursePrerequisiteLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static CoursePrerequisite Create(long courseId, long prerequisiteCourseId)
    {
        var entity = new CoursePrerequisite();
        entity.CourseId = courseId;
        entity.PrerequisiteCourseId = prerequisiteCourseId;
        return entity;
    }
}
