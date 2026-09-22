using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر Major؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class MajorLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static Major Create(long departmentId, string code, string title)
    {
        var entity = new Major();
        if (departmentId <= 0)
        {
            throw new ArgumentException("شناسه گروه آموزشی برای رشته باید معتبر باشد.", nameof(departmentId));
        }

        entity.DepartmentId = departmentId;
        entity.Code = code.Trim();
        entity.Title = title.Trim();
        entity.IsActive = true;
        return entity;
    }

    public static void Activate(Major entity)
    {
        entity.IsActive = true;
    }
    public static void Deactivate(Major entity)
    {
        entity.IsActive = false;
    }
}
