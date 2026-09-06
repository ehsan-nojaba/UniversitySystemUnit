namespace UniversitySystem.Application.Common.Exceptions;

/// <summary>
/// Thrown when the current user does not have permission to perform an action.
/// Maps to HTTP 403 Forbidden in the API layer.
/// </summary>
public sealed class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException()
        : base("You do not have permission to perform this action.")
    {
    }
}
