using UniversitySystem.Application.Features.Auth.Commands.Login;

namespace UniversitySystem.UnitTests.Authentication;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenUsernameAndPasswordAreProvided_IsValid()
    {
        // Arrange
        var command = new LoginCommand("admin_user", "SecurePass123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WhenUsernameIsEmpty_FailsValidation(string? username)
    {
        // Arrange
        var command = new LoginCommand(username!, "ValidPassword123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginCommand.Username));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WhenPasswordIsEmpty_FailsValidation(string? password)
    {
        // Arrange
        var command = new LoginCommand("valid_user", password!);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginCommand.Password));
    }
}
