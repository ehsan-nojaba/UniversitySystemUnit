namespace UniversitySystem.Domain.Entities;

/// <summary>
/// موجودیت واسط کاربر-نقش: انتساب یک نقش مشخص به یک کاربر خاص در سیستم.
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
