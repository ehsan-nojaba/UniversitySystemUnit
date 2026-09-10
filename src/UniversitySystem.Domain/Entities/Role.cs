using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

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
