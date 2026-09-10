namespace UniversitySystem.Domain.Entities;

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
