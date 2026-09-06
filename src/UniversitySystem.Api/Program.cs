using UniversitySystem.Api;
using UniversitySystem.Application;
using UniversitySystem.Infrastructure;
using UniversitySystem.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ── Composition Root ───────────────────────────────────────────────────────────
// Each layer registers its own dependencies. No implementation details leak here.

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
/// Expose Program class for WebApplicationFactory in Integration Tests.
/// </summary>
public partial class Program;
