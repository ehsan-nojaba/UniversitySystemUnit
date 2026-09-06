using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Common;

namespace UniversitySystem.Persistence.Data;

/// <summary>
/// The central EF Core DbContext for the UniversitySystem.
///
/// Responsibilities:
///   - Acts as the single Unit of Work for all database operations.
///   - Applies all entity configurations from this assembly via Fluent API.
///   - Automatically populates audit fields on <see cref="SaveChangesAsync"/>.
///
/// Business DbSets will be added in subsequent tasks as Entities are defined.
/// </summary>
public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider)
        : base(options)
    {
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
    }

    // ── Business DbSets ──────────────────────────────────────────────────────────
    // Will be added here in Task 4 as each Entity is defined.
    // Example: public DbSet<Student> Students => Set<Student>();

    // ────────────────────────────────────────────────────────────────────────────

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Discovers and applies all IEntityTypeConfiguration<T> classes in this assembly.
        // Entity configurations must never be written inline here.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Saves all pending changes and automatically populates audit fields
    /// for any <see cref="BaseAuditableEntity"/> that was added or modified.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        PopulateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    // ── Auditing ─────────────────────────────────────────────────────────────────

    private void PopulateAuditFields()
    {
        var utcNow = _dateTimeProvider.UtcNow;
        var currentUserId = _currentUserService.UserId;

        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.CreatedBy = currentUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedAt = utcNow;
                    entry.Entity.LastModifiedBy = currentUserId;
                    break;
            }
        }
    }
}
