namespace UniversitySystem.Domain.Entities;

/// <summary>
/// رابط میان حساب کاربر و نقش؛ یک کاربر می‌تواند چند نقش داشته باشد.
/// </summary>
public class UserRole
{
    public long UserId { get; private set; }
    public long RoleId { get; private set; }

    private UserRole() { }

    public UserRole(long userId, long roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}
