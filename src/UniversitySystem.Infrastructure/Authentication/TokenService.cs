using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Infrastructure.Authentication;

/// <summary>
/// Generates JSON Web Tokens (JWT) using HMAC-SHA256 according to application settings.
/// Embeds standard claims: sub (UserId), unique_name/name (Username), given_name (FullName), roles, and jti.
/// </summary>
public sealed class TokenService : ITokenService
{
    private const string DevelopmentFallbackSecret = "DEVELOPMENT_SECRET_KEY_NOT_FOR_PRODUCTION_USE_AT_LEAST_32_BYTES_LONG_12345";
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    /// <inheritdoc />
    public (string AccessToken, DateTime ExpiresAt) GenerateToken(User user, IEnumerable<string> roles)
    {
        var secretKey = !string.IsNullOrWhiteSpace(_jwtSettings.SecretKey) && _jwtSettings.SecretKey.Length >= 32
            ? _jwtSettings.SecretKey
            : DevelopmentFallbackSecret;

        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes > 0 ? _jwtSettings.ExpirationMinutes : 60);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.GivenName, user.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("role", role));
            }
        }

        var issuer = !string.IsNullOrWhiteSpace(_jwtSettings.Issuer) ? _jwtSettings.Issuer : "UniversitySystem";
        var audience = !string.IsNullOrWhiteSpace(_jwtSettings.Audience) ? _jwtSettings.Audience : "UniversitySystem";

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return (tokenString, expiresAt);
    }
}
