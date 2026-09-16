using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// نقش دسترسی مانند دانشجو، استاد یا آموزش؛ تعیین می‌کند کاربر اجازه استفاده از کدام API را دارد.
/// </summary>
public class Role : BaseAuditableEntity
{
    public string Name { get; private set; } = default!;

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private Role() { }

    public Role(string name)
    {
        Name = name.Trim();
    }
}
