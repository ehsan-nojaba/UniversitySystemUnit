using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر ProfessorCourse؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class ProfessorCourseLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static ProfessorCourse Create(long professorId, long courseId)
    {
        var entity = new ProfessorCourse();
        if (professorId <= 0 || courseId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(courseId));
        }

        entity.ProfessorId = professorId;
        entity.CourseId = courseId;
        return entity;
    }
}
