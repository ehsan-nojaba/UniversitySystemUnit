using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر Curriculum؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class CurriculumLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static Curriculum Create(long majorId, string title, string version)
    {
        var entity = new Curriculum();
        entity.MajorId = majorId;
        entity.Title = title.Trim();
        entity.Version = version.Trim();
        entity.IsActive = true;
        return entity;
    }

    public static void AddCourse(Curriculum entity, long courseId, int recommendedTerm, bool isRequired)
    {
        if (courseId <= 0 || recommendedTerm <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(courseId));
        }

        bool alreadyAdded = entity.CurriculumCourses.Any(cc => cc.CourseId == courseId);
        if (!alreadyAdded)
        {
            entity.CurriculumCourses.Add(CurriculumCourseLogic.Create(entity.Id, courseId, recommendedTerm, isRequired));
        }
    }

    public static void UpdateCourse(Curriculum entity, long courseId, int recommendedTerm, bool isRequired)
    {
        CurriculumCourseLogic.Update(entity.CurriculumCourses.Single(c => c.CourseId == courseId), recommendedTerm, isRequired);
    }

    public static void RemoveCourse(Curriculum entity, long courseId)
    {
        var item = entity.CurriculumCourses.SingleOrDefault(c => c.CourseId == courseId);
        if (item is not null)
        {
            entity.CurriculumCourses.Remove(item);
        }
    }

    public static void Activate(Curriculum entity)
    {
        entity.IsActive = true;
    }
    public static void Deactivate(Curriculum entity)
    {
        entity.IsActive = false;
    }
}
