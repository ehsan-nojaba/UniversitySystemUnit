using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// نیم‌سال تحصیلی با کد، عنوان و تاریخ شروع و پایان؛ درخواست‌ها و ارائه‌ها برای آن ثبت می‌شوند.
/// </summary>
public class AcademicTerm : BaseAuditableEntity
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}
