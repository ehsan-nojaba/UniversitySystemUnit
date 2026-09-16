namespace UniversitySystem.Domain.Common;

/// <summary>
/// اطلاعات مشترک ایجاد و آخرین ویرایش رکورد، شامل زمان و کاربر انجام‌دهنده.
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}
