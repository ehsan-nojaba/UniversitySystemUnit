using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر User؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class UserLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static User Create(string username, string passwordHash, string firstName, string lastName)
    {
        var entity = new User();
        entity.Username = username.Trim();
        entity.PasswordHash = passwordHash;
        entity.FirstName = firstName.Trim();
        entity.LastName = lastName.Trim();
        entity.IsActive = true;
        return entity;
    }

    public static string GetFullName(User entity)
    {
        return $"{entity.FirstName} {entity.LastName}";
    }
    public static void Activate(User entity)
    {
        entity.IsActive = true;
    }
    public static void Deactivate(User entity)
    {
        entity.IsActive = false;
    }
    public static void AssignRole(User entity, Role role)
    {
        bool alreadyAssigned = entity.UserRoles.Any(ur => ur.RoleId == role.Id);
        if (!alreadyAssigned)
        {
            entity.UserRoles.Add(UserRoleLogic.Create(entity.Id, role.Id));
        }
    }
}
