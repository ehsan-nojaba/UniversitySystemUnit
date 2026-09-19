using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// نقش دسترسی مانند دانشجو، استاد یا آموزش؛ تعیین می‌کند کاربر اجازه استفاده از کدام API را دارد.
/// </summary>
public class Role : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
