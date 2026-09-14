using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// موجودیت نقش کاربری: مشخص‌کننده نقش‌ها و سطوح دسترسی کاربران در سامانه دانشگاهی (مانند دانشجو، استاد، آموزش).
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
