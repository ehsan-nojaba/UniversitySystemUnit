using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر CourseOffering؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class CourseOfferingLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static CourseOffering Create(long courseId, long academicTermId, int capacity)
    {
        var entity = new CourseOffering();
        if (courseId <= 0)
        {
            throw new ArgumentException("شناسه درس ارائه باید معتبر باشد.", nameof(courseId));
        }

        if (academicTermId <= 0)
        {
            throw new ArgumentException("شناسه ترم تحصیلی ارائه باید معتبر باشد.", nameof(academicTermId));
        }

        if (capacity <= 0)
        {
            throw new ArgumentException("ظرفیت باید عددی مثبت باشد.", nameof(capacity));
        }

        entity.CourseId = courseId;
        entity.AcademicTermId = academicTermId;
        entity.Capacity = capacity;
        entity.IsActive = true;
        return entity;
    }

    public static void UpdateCapacity(CourseOffering entity, int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("ظرفیت باید عددی مثبت باشد.", nameof(capacity));
        }

        entity.Capacity = capacity;
    }

    public static void Activate(CourseOffering entity)
    {
        entity.IsActive = true;
    }
    public static void Deactivate(CourseOffering entity)
    {
        entity.IsActive = false;
    }
    public static void FinalizePlanning(CourseOffering entity)
    {
        entity.IsFinalized = true;
    }
    public static void ReopenPlanning(CourseOffering entity)
    {
        entity.IsFinalized = false;
    }
}
