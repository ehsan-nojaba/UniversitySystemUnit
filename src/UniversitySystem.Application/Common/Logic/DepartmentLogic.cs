using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر Department؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class DepartmentLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static Department Create(long facultyId, string code, string title)
    {
        var entity = new Department();
        if (facultyId <= 0)
        {
            throw new ArgumentException("Department must have a valid Faculty.", nameof(facultyId));
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Department code cannot be empty.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Department title cannot be empty.", nameof(title));
        }

        entity.FacultyId = facultyId;
        entity.Code = code.Trim();
        entity.Title = title.Trim();
        entity.IsActive = true;
        return entity;
    }

    public static void Activate(Department entity)
    {
        entity.IsActive = true;
    }
    public static void Deactivate(Department entity)
    {
        entity.IsActive = false;
    }
}
