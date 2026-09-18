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
/// سرویس‌های Infrastructure را در DI ثبت می‌کند؛ نقطه اتصال قراردادها و پیاده‌سازی‌های این لایه است.
/// </summary>
public static class DependencyInjection
{
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
        // تولید و اعتبارسنجی توکن از یک تنظیم معتبر استفاده می‌کنند؛ کلید پیش‌فرض مخفی وجود ندارد.
        services.AddOptions<JwtSettings>().Bind(jwtSection)
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.SecretKey) && Encoding.UTF8.GetByteCount(settings.SecretKey) >= 32, "کلید JWT باید حداقل ۳۲ بایت باشد.")
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.Issuer) && !string.IsNullOrWhiteSpace(settings.Audience), "صادرکننده و مخاطب JWT باید مشخص باشند.")
            .Validate(settings => settings.ExpirationMinutes > 0, "مدت اعتبار JWT باید مثبت باشد.")
            .ValidateOnStart();

        var jwtSettings = jwtSection.Get<JwtSettings>() ?? new JwtSettings();

        // ── 4. JWT Bearer Authentication ───────────────────────────────────────
        var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

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
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
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
