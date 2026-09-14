using UniversitySystem.Application.Features.StudentPreRegistration.Commands.SubmitStudentPreRegistration;

namespace UniversitySystem.UnitTests.StudentPreRegistration;

public class SubmitStudentPreRegistrationCommandValidatorTests
{
    private readonly SubmitStudentPreRegistrationCommandValidator _validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(999)]
    public void Validate_WhenAcademicTermIdIsPositive_PassesValidation(long termId)
    {
        // Arrange
        var command = new SubmitStudentPreRegistrationCommand(termId);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Validate_WhenAcademicTermIdIsZeroOrNegative_FailsValidation(long termId)
    {
        // Arrange
        var command = new SubmitStudentPreRegistrationCommand(termId);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(SubmitStudentPreRegistrationCommand.AcademicTermId));
    }
}
