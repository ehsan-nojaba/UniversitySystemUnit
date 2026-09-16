namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// قرارداد هش و بررسی رمز؛ سرویس ورود را از روش فنی هش مستقل می‌کند.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>Hashes a plain-text password and returns the hash string.</summary>
    string Hash(string password);

    /// <summary>
    /// Verifies a plain-text password against a previously produced hash.
    /// Returns <c>true</c> when the password matches; <c>false</c> otherwise.
    /// </summary>
    bool Verify(string password, string passwordHash);
}
