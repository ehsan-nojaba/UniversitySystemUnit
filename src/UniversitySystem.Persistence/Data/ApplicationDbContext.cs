using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Common;

namespace UniversitySystem.Persistence.Data;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
    }

    // ── Business DbSets ──────────────────────────────────────────────────────────
    // Will be added here as each Entity is defined.
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
        var now = _dateTimeProvider.Now;
        var currentUserId = _currentUserService.UserId;

        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedAt = now;
                    entry.Entity.LastModifiedBy = currentUserId;
                    break;
            }
        }
    }
}
