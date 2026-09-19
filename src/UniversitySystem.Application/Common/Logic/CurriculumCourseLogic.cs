using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر CurriculumCourse؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class CurriculumCourseLogic
{
    public static void Update(CurriculumCourse entity, int recommendedTerm, bool isRequired)
    {
        if (recommendedTerm <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(recommendedTerm));
        }

        entity.RecommendedTerm = recommendedTerm;
        entity.IsRequired = isRequired;
    }

    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static CurriculumCourse Create(long curriculumId, long courseId, int recommendedTerm, bool isRequired)
    {
        var entity = new CurriculumCourse();
        entity.CurriculumId = curriculumId;
        entity.CourseId = courseId;
        entity.RecommendedTerm = recommendedTerm;
        entity.IsRequired = isRequired;
        return entity;
    }
}
