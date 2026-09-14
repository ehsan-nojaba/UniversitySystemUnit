using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// موجودیت ترم تحصیلی: نمایانگر یک نیم‌سال تحصیلی در دانشگاه، بازه زمانی شروع و پایان و وضعیت فعال بودن آن.
/// </summary>
public class AcademicTerm : BaseAuditableEntity
{
    public string Code { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; }

    private AcademicTerm() { }

    public AcademicTerm(string code, string title, DateTime startDate, DateTime endDate)
    {
        Code = code.Trim();
        Title = title.Trim();
        StartDate = startDate;
        EndDate = endDate;
        IsActive = true;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
