using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر AcademicTerm؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class AcademicTermLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static AcademicTerm Create(string code, string title, DateTime startDate, DateTime endDate)
    {
        var entity = new AcademicTerm();
        entity.Code = code.Trim();
        entity.Title = title.Trim();
        entity.StartDate = startDate;
        entity.EndDate = endDate;
        entity.IsActive = true;
        return entity;
    }

    public static void Activate(AcademicTerm entity) => entity.IsActive = true;
    public static void Deactivate(AcademicTerm entity) => entity.IsActive = false;
}
