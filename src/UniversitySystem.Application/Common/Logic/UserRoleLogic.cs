using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر UserRole؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class UserRoleLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static UserRole Create(long userId, long roleId)
    {
        var entity = new UserRole();
        entity.UserId = userId;
        entity.RoleId = roleId;
        return entity;
    }
}
