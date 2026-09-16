namespace UniversitySystem.Application.Features.Auth.Commands.Login;

/// <summary>
/// پاسخ ورود موفق شامل توکن دسترسی، زمان انقضا، شناسه و نام کاربر و نقش‌های او.
/// </summary>
public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public long UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string[] Roles { get; set; } = [];
}
