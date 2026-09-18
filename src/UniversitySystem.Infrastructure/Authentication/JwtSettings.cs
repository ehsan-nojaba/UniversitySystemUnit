namespace UniversitySystem.Infrastructure.Authentication;

/// <summary>
/// تنظیمات توکن شامل صادرکننده، مخاطب، کلید امضا و مدت اعتبار؛ مدل آموزشی یا جدول دیتابیس نیست.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";

    /// <summary>
    /// کلید امضای HMAC-SHA256؛ باید حداقل ۳۲ بایت باشد و در محیط اصلی از تنظیمات محرمانه خوانده شود.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// نام صادرکننده معتبر توکن؛ هنگام تولید و اعتبارسنجی یکسان است.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// مخاطب معتبر توکن؛ مشخص می‌کند توکن برای کدام سامانه صادر شده است.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// مدت اعتبار توکن به دقیقه؛ مقدار باید مثبت باشد.
    /// </summary>
    public int ExpirationMinutes { get; set; } = 60;
}
