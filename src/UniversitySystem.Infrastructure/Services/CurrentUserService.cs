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
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    /// <inheritdoc />
    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    /// <inheritdoc />
    public IReadOnlyList<string> Roles =>
        _httpContextAccessor.HttpContext?.User?
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .Distinct()
            .ToList()
            .AsReadOnly()
        ?? (IReadOnlyList<string>)Array.Empty<string>();
}
