using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر Student؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class StudentLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static Student Create(long userId, string studentNumber, long majorId, int entryYear)
    {
        var entity = new Student();
        entity.UserId = userId;
        entity.StudentNumber = studentNumber.Trim();
        entity.MajorId = majorId;
        entity.EntryYear = entryYear;
        return entity;
    }
}
