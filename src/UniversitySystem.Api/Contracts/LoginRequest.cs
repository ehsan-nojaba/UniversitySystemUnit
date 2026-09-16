namespace UniversitySystem.Api.Contracts;

/// <summary>نام کاربری و رمز عبور دریافتی از فرم ورود.</summary>
public sealed class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
