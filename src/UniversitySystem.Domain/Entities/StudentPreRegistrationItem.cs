using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// یک درس در درخواست پیش‌انتخاب؛ شناسه درس و اولویت دانشجو را نگه می‌دارد و ثبت‌نام قطعی نیست.
/// </summary>
public class StudentPreRegistrationItem : BaseAuditableEntity
{
    public long StudentPreRegistrationId { get; set; }
    public long CourseId { get; set; }
    public int Priority { get; set; }
    public StudentPreRegistration StudentPreRegistration { get; set; } = default!;
    public Course Course { get; set; } = default!;
}
