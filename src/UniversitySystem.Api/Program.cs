using UniversitySystem.Api;
using UniversitySystem.Application;
using UniversitySystem.Infrastructure;
using UniversitySystem.Persistence;

var builder = WebApplication.CreateBuilder(args);

// نقطه شروع برنامه؛ هر لایه سرویس‌های خودش را ثبت می‌کند و API آن‌ها را به هم متصل می‌کند.

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApiServices();

// ──────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Middleware Pipeline ────────────────────────────────────────────────────────
// 1. Centralized global exception handler
app.UseExceptionHandler();

// 2. Swagger / OpenAPI (Development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "University System API v1");
    });
}

// 3. Security & Protocol
app.UseHttpsRedirection();

// 4. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 5. Endpoints
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

/// <summary>
/// مدل کمکی Program؛ مسئولیت آن در راهنمای فارسی پروژه توضیح داده شده است.
/// </summary>
public partial class Program;
