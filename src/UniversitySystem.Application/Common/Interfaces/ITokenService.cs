using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// قرارداد تولید توکن ورود و زمان انقضا؛ تنظیمات امضای JWT در Infrastructure قرار دارند.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a signed JWT access token for the given user and assigned role names.
    /// </summary>
    /// <param name="user">The authenticated user entity.</param>
    /// <param name="roles">The list of assigned role names.</param>
    /// <returns>A tuple containing the serialized token string and UTC expiration timestamp.</returns>
    (string AccessToken, DateTime ExpiresAt) GenerateToken(User user, IEnumerable<string> roles);
}
