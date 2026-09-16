using FluentValidation.Results;

namespace UniversitySystem.Application.Common.Exceptions;

/// <summary>
/// خطای ورودی نامعتبر همراه خطاهای هر فیلد؛ در API به پاسخ ۴۰۰ تبدیل می‌شود.
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
