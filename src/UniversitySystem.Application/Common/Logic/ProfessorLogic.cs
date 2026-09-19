using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر Professor؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class ProfessorLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static Professor Create(long userId, string personnelCode)
    {
        var entity = new Professor();
        entity.UserId = userId;
        entity.PersonnelCode = personnelCode.Trim();
        return entity;
    }
}
