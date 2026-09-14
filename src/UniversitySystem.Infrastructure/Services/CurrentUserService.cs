using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UniversitySystem.Application.Common.Interfaces;

namespace UniversitySystem.Infrastructure.Services;

/// <summary>
/// Implements <see cref="ICurrentUserService"/> using <see cref="IHttpContextAccessor"/>.
/// Extracts user identity, authentication state, and assigned roles from the current HTTP request.
/// Designed to be completely null-safe for unauthenticated/anonymous requests and background executions.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public string? UserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

    /// <inheritdoc />
    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    /// <inheritdoc />
    public IReadOnlyList<string> Roles
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                return Array.Empty<string>();

            var rolesFromUri = user.FindAll(ClaimTypes.Role).Select(c => c.Value);
            var rolesFromShort = user.FindAll("role").Select(c => c.Value);

            return rolesFromUri
                .Concat(rolesFromShort)
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList()
                .AsReadOnly();
        }
    }

    /// <inheritdoc />
    public bool IsInRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            return false;

        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !IsAuthenticated)
            return false;

        return user.IsInRole(role) || Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }
}
