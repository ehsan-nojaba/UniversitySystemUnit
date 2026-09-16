namespace UniversitySystem.Domain.Common;

/// <summary>
/// شناسه مشترک موجودیت‌های دامنه؛ هویت هر رکورد را مشخص می‌کند.
/// </summary>
public abstract class BaseEntity
{
    public long Id { get; set; }
}
