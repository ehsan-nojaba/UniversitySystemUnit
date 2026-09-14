using UniversitySystem.Infrastructure.Authentication;

namespace UniversitySystem.UnitTests.Authentication;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void Hash_GivenValidPassword_ReturnsHashedStringDifferentFromPassword()
    {
        // Arrange
        const string password = "SecretPassword123!";

        // Act
        var hash = _hasher.Hash(password);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.NotEqual(password, hash);
        Assert.Contains(":", hash);
    }

    [Fact]
    public void Verify_GivenMatchingPasswordAndHash_ReturnsTrue()
    {
        // Arrange
        const string password = "MySecurePassword#2026";
        var hash = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Verify_GivenWrongPassword_ReturnsFalse()
    {
        // Arrange
        const string password = "CorrectPassword";
        const string wrongPassword = "WrongPassword";
        var hash = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify(wrongPassword, hash);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Verify_GivenEmptyOrNullInput_ReturnsFalse(string? input)
    {
        // Arrange
        var hash = _hasher.Hash("validPass123");

        // Act & Assert
        Assert.False(_hasher.Verify(input!, hash));
        Assert.False(_hasher.Verify("validPass123", input!));
    }
}
