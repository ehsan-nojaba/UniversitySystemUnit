namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// Provides the identity of the currently authenticated user.
///
/// Used by the Persistence layer for audit trail population (CreatedBy, LastModifiedBy).
/// Implementations must be null-safe: when no user is authenticated
/// (e.g., background jobs or anonymous requests), <see cref="UserId"/> returns <c>null</c>.
///
/// Implementation: <c>UniversitySystem.Infrastructure.Services.CurrentUserService</c>
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Returns the identifier of the currently authenticated user,
    /// or <c>null</c> if no user is authenticated.
    /// </summary>
    string? UserId { get; }
}
