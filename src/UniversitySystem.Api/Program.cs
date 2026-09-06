using UniversitySystem.Application;
using UniversitySystem.Infrastructure;
using UniversitySystem.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ── Composition Root ───────────────────────────────────────────────────────────
// Each layer exposes a single extension method that registers its own services.
// No layer's internals leak into this file.

builder.Services.AddApplication();
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistence(builder.Configuration);

// ── API ────────────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ──────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
