using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر TeachingAssignment؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class TeachingAssignmentLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static TeachingAssignment Create(long courseOfferingId, long professorId, DateTime assignedAt)
    {
        var entity = new TeachingAssignment();
        if (courseOfferingId <= 0)
        {
            throw new ArgumentException("شناسه ارائه درس برای تخصیص استاد باید معتبر باشد.", nameof(courseOfferingId));
        }

        if (professorId <= 0)
        {
            throw new ArgumentException("شناسه استاد برای تخصیص تدریس باید معتبر باشد.", nameof(professorId));
        }

        entity.CourseOfferingId = courseOfferingId;
        entity.ProfessorId = professorId;
        entity.AssignedAt = assignedAt;
        return entity;
    }
}
