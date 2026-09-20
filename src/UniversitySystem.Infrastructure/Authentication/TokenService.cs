using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.Infrastructure.Authentication;

/// <summary>
/// توکن JWT را با شناسه و نقش‌های کاربر و زمان انقضا تولید می‌کند.
/// </summary>
public sealed class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    /// <inheritdoc />
    public (string AccessToken, DateTime ExpiresAt) GenerateToken(User user, IEnumerable<string> roles)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var issuedAt = DateTime.UtcNow;
        var expiresAt = issuedAt.AddMinutes(_jwtSettings.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.GivenName, UserLogic.GetFullName(user)),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles.Distinct(StringComparer.Ordinal))
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("role", role));
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            IssuedAt = issuedAt,
            NotBefore = issuedAt,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return (tokenString, expiresAt);
    }
}

