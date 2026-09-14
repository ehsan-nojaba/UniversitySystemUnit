namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// Abstracts password hashing and verification.
/// Implementation lives in Infrastructure; Application never sees the algorithm details.
/// Plain-text passwords must never be logged or stored.
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
