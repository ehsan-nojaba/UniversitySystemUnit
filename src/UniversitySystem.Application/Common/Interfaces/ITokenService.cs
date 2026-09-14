using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// Generates JSON Web Tokens (JWT) for authenticated users.
/// Implemented in Infrastructure using configured JWT parameters.
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
