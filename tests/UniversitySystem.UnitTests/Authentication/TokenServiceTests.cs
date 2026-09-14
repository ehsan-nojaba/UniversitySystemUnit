using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Infrastructure.Authentication;

namespace UniversitySystem.UnitTests.Authentication;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private readonly JwtSettings _jwtSettings;

    public TokenServiceTests()
    {
        _jwtSettings = new JwtSettings
        {
            SecretKey = "super_secret_test_key_which_is_at_least_32_bytes_long!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpirationMinutes = 30
        };

        var options = Options.Create(_jwtSettings);
        _tokenService = new TokenService(options);
    }

    [Fact]
    public void GenerateToken_ReturnsValidJwtAndFutureExpiration()
    {
        // Arrange
        var user = new User("johndoe", "hashedpass", "John", "Doe");
        var roles = new List<string> { "Student", "TeachingAssistant" };

        // Act
        var (token, expiresAt) = _tokenService.GenerateToken(user, roles);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.True(expiresAt > DateTime.UtcNow);

        // Decode and verify claims
        var handler = new JwtSecurityTokenHandler();
        Assert.True(handler.CanReadToken(token));

        var jwt = handler.ReadJwtToken(token);
        Assert.Equal(_jwtSettings.Issuer, jwt.Issuer);
        Assert.Contains(_jwtSettings.Audience, jwt.Audiences);

        var claims = jwt.Claims.ToList();
        Assert.Contains(claims, c => (c.Type == JwtRegisteredClaimNames.UniqueName || c.Type == "unique_name") && c.Value == "johndoe");
        Assert.Contains(claims, c => (c.Type == ClaimTypes.GivenName || c.Type == "given_name") && c.Value == "John Doe");
        Assert.Contains(claims, c => (c.Type == ClaimTypes.Role || c.Type == "role") && c.Value == "Student");
        Assert.Contains(claims, c => (c.Type == ClaimTypes.Role || c.Type == "role") && c.Value == "TeachingAssistant");
    }
}
