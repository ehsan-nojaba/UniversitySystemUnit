using FluentValidation.Results;
using UniversitySystem.Application.Common.Exceptions;
using Xunit;

namespace UniversitySystem.UnitTests.Common;

public class ValidationExceptionTests
{
    [Fact]
    public void DefaultConstructor_InitializesEmptyErrorDictionary()
    {
        // Act
        var exception = new ValidationException();

        // Assert
        Assert.NotNull(exception.Errors);
        Assert.Empty(exception.Errors);
    }

    [Fact]
    public void FailuresConstructor_GroupsErrorsByPropertyName()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new("Email", "Email is required"),
            new("Email", "Email must be a valid email address"),
            new("Password", "Password is required")
        };

        // Act
        var exception = new ValidationException(failures);

        // Assert
        Assert.Equal(2, exception.Errors.Count);
        Assert.Equal(2, exception.Errors["Email"].Length);
        Assert.Single(exception.Errors["Password"]);
        Assert.Contains("Email is required", exception.Errors["Email"]);
        Assert.Contains("Email must be a valid email address", exception.Errors["Email"]);
        Assert.Contains("Password is required", exception.Errors["Password"]);
    }
}
