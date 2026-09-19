using UniversitySystem.Domain.Common;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// درخواست پیش‌انتخاب دانشجو برای یک ترم؛ درس‌های موردنیاز و وضعیت پیش‌نویس یا ارسال‌شده را مدیریت می‌کند.
/// </summary>
public class StudentPreRegistration : BaseAuditableEntity
{
    public long StudentId { get; set; }
    public long AcademicTermId { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public Student Student { get; set; } = default!;
    public AcademicTerm AcademicTerm { get; set; } = default!;
    public ICollection<StudentPreRegistrationItem> Items { get; set; } = new List<StudentPreRegistrationItem>();
}
