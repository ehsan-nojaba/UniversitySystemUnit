using FluentValidation.Results;

namespace UniversitySystem.Application.Common.Exceptions;

/// <summary>
/// Thrown when one or more FluentValidation rules fail for a request.
/// Carries a structured collection of field-level errors so that
/// the API layer can translate them into a RFC 7807 ProblemDetails response.
/// </summary>
public sealed class ValidationException : Exception
{
    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(failure => failure.PropertyName, failure => failure.ErrorMessage)
            .ToDictionary(group => group.Key, group => group.ToArray());
    }

    /// <summary>
    /// Field-keyed dictionary of validation error messages.
    /// Key: property name. Value: one or more error messages for that property.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }
}
