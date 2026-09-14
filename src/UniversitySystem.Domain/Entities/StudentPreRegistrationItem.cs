using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// موجودیت آیتم پیش‌ثبت‌نام: هر درس انتخاب شده توسط دانشجو در فرم پیش‌ثبت‌نام به همراه اولویت ترجیحی (Priority).
/// </summary>
public class StudentPreRegistrationItem : BaseAuditableEntity
{
    public long StudentPreRegistrationId { get; private set; }
    public long CourseId { get; private set; }
    public int Priority { get; private set; }

    public StudentPreRegistration StudentPreRegistration { get; private set; } = default!;
    public Course Course { get; private set; } = default!;

    private StudentPreRegistrationItem() { }

    internal StudentPreRegistrationItem(long studentPreRegistrationId, long courseId, int priority)
    {
        StudentPreRegistrationId = studentPreRegistrationId;
        CourseId = courseId;
        Priority = priority;
    }

    internal void UpdatePriority(int priority)
    {
        Priority = priority;
    }
}
