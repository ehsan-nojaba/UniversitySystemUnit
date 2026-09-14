namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// Provides identity information for the currently authenticated user.
///
/// Used across Application and Persistence layers (e.g. audit fields).
/// Must be null-safe: for anonymous requests or background workers,
/// <see cref="UserId"/> returns <c>null</c>, <see cref="IsAuthenticated"/> returns <c>false</c>,
/// and <see cref="Roles"/> returns an empty list.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the unique identifier of the currently authenticated user, or <c>null</c> if unauthenticated.
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Indicates whether the current request is from an authenticated user.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the list of role names associated with the current user.
    /// </summary>
    IReadOnlyList<string> Roles { get; }

    /// <summary>
    /// Determines whether the current authenticated user belongs to the specified role.
    /// </summary>
    bool IsInRole(string role);
}
