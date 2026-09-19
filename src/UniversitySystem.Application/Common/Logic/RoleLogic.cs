using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر Role؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class RoleLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static Role Create(string name)
    {
        var entity = new Role();
        entity.Name = name.Trim();
        return entity;
    }
}
