using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// حساب ورود کاربر؛ نام کاربری، هش رمز و وضعیت فعال بودن را نگه می‌دارد و به نقش‌ها و پروفایل دانشجو یا استاد متصل است.
/// </summary>
public class User : BaseAuditableEntity
{
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public bool IsActive { get; set; }
    public Student? Student { get; set; }
    public Professor? Professor { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
