using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Infrastructure.Authentication;
using UniversitySystem.Infrastructure.Services;

namespace UniversitySystem.Infrastructure;

/// <summary>
/// Registers Infrastructure layer services into the ASP.NET Core dependency injection container.
/// </summary>
public static class DependencyInjection
{
    private const string DevelopmentFallbackSecret = "DEVELOPMENT_SECRET_KEY_NOT_FOR_PRODUCTION_USE_AT_LEAST_32_BYTES_LONG_12345";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // ── 1. HTTP Context Accessor ───────────────────────────────────────────
        services.AddHttpContextAccessor();

        // ── 2. Cross-Cutting & Utility Services ────────────────────────────────
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();

        // ── 3. JWT Configuration (Options Pattern) ─────────────────────────────
        var jwtSection = configuration.GetSection(JwtSettings.SectionName);
        services.Configure<JwtSettings>(jwtSection);

        var jwtSettings = jwtSection.Get<JwtSettings>() ?? new JwtSettings();

        // ── 4. JWT Bearer Authentication ───────────────────────────────────────
        var secretKey = !string.IsNullOrWhiteSpace(jwtSettings.SecretKey) && jwtSettings.SecretKey.Length >= 32
            ? jwtSettings.SecretKey
            : DevelopmentFallbackSecret;

        var keyBytes = Encoding.UTF8.GetBytes(secretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = !string.IsNullOrWhiteSpace(jwtSettings.Issuer) ? jwtSettings.Issuer : "UniversitySystem",
                ValidAudience = !string.IsNullOrWhiteSpace(jwtSettings.Audience) ? jwtSettings.Audience : "UniversitySystem",
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name,
                ClockSkew = TimeSpan.Zero
            };
        });

        // ── 5. Authorization Foundation ────────────────────────────────────────
        services.AddAuthorization();

        return services;
    }
}
