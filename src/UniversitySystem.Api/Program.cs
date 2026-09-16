using Scalar.AspNetCore;
using UniversitySystem.Api;
using UniversitySystem.Application;
using UniversitySystem.Infrastructure;
using UniversitySystem.Persistence;

var builder = WebApplication.CreateBuilder(args);
// نقطه شروع برنامه؛ هر لایه سرویس‌های خودش را ثبت می‌کند و API آن‌ها را به هم متصل می‌کند.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);
// ──────────────────────────────────────────────────────────────────────────────
var app = builder.Build();
// ── Middleware Pipeline ────────────────────────────────────────────────────────
// 1. Centralized global exception handler
app.UseExceptionHandler();
// مستندات تعاملی فقط در محیط توسعه نمایش داده می‌شوند.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options.WithTitle("University System API").AddPreferredSecuritySchemes("Bearer"));
}

// 3. Security & Protocol
app.UseHttpsRedirection();
// 4. Authentication & Authorization
app.UseCors("Ui");
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
