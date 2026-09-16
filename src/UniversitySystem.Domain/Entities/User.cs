using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// حساب ورود کاربر؛ نام کاربری، هش رمز و وضعیت فعال بودن را نگه می‌دارد و به نقش‌ها و پروفایل دانشجو یا استاد متصل است.
/// </summary>
public class User : BaseAuditableEntity
{
    public string Username { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public bool IsActive { get; private set; }

    public Student? Student { get; private set; }
    public Professor? Professor { get; private set; }

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private User() { }

    public User(string username, string passwordHash, string firstName, string lastName)
    {
        Username = username.Trim();
        PasswordHash = passwordHash;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        IsActive = true;
    }

    public string FullName => $"{FirstName} {LastName}";

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void AssignRole(Role role)
    {
        bool alreadyAssigned = _userRoles.Any(ur => ur.RoleId == role.Id);
        if (!alreadyAssigned)
            _userRoles.Add(new UserRole(Id, role.Id));
    }
}
