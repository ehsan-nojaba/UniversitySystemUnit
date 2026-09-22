using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر Faculty؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class FacultyLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static Faculty Create(string code, string title)
    {
        var entity = new Faculty();
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("کد دانشکده نمی‌تواند خالی باشد.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("عنوان دانشکده نمی‌تواند خالی باشد.", nameof(title));
        }

        entity.Code = code.Trim();
        entity.Title = title.Trim();
        entity.IsActive = true;
        return entity;
    }

    public static void Activate(Faculty entity)
    {
        entity.IsActive = true;
    }
    public static void Deactivate(Faculty entity)
    {
        entity.IsActive = false;
    }
}
