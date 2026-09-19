using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر Course؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class CourseLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static Course Create(string code, string title, int credits)
    {
        var entity = new Course();
        entity.Code = code.Trim();
        entity.Title = title.Trim();
        entity.Credits = credits;
        entity.IsActive = true;
        return entity;
    }

    public static void AddPrerequisite(Course entity, long prerequisiteCourseId)
    {
        bool alreadyExists = entity.Prerequisites.Any(p => p.PrerequisiteCourseId == prerequisiteCourseId);
        if (!alreadyExists)
        {
            entity.Prerequisites.Add(CoursePrerequisiteLogic.Create(entity.Id, prerequisiteCourseId));
        }
    }

    public static void Activate(Course entity) => entity.IsActive = true;
    public static void Deactivate(Course entity) => entity.IsActive = false;
}
