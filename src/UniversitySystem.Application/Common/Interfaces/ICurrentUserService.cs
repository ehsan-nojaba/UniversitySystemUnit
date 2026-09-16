namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// قرارداد دریافت شناسه و نقش کاربر جاری؛ مانع نیاز سرویس آموزشی به HttpContext می‌شود.
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
