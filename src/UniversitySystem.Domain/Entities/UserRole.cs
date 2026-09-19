namespace UniversitySystem.Domain.Entities;
/// <summary>
/// رابط میان حساب کاربر و نقش؛ یک کاربر می‌تواند چند نقش داشته باشد.
/// </summary>
public class UserRole
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
}
