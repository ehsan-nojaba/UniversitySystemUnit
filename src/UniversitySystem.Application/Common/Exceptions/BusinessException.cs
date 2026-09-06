namespace UniversitySystem.Application.Common.Exceptions;

/// <summary>
/// Thrown when a domain or business rule is violated.
/// Maps to HTTP 422 Unprocessable Entity in the API layer.
/// Use this for invariants that cannot be expressed as simple field validators,
/// e.g., "A student cannot register for two conflicting courses."
/// </summary>
public sealed class BusinessException : Exception
{
    public BusinessException(string message)
        : base(message)
    {
    }
}
