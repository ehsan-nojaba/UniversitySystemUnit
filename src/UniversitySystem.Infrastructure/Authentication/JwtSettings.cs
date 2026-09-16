namespace UniversitySystem.Infrastructure.Authentication;

/// <summary>
/// تنظیمات توکن شامل صادرکننده، مخاطب، کلید امضا و مدت اعتبار؛ مدل آموزشی یا جدول دیتابیس نیست.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";

    /// <summary>
    /// Secret key used to sign and verify JSON Web Tokens.
    /// Must be at least 256 bits (32 characters) for HMAC-SHA256.
    /// In production, provide via environment variables, User Secrets, or Azure Key Vault.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// The valid issuer of the token (e.g. "UniversitySystem").
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// The valid audience of the token (e.g. "UniversitySystem").
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Token lifetime in minutes before expiration.
    /// </summary>
    public int ExpirationMinutes { get; set; } = 60;
}
