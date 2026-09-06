namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// Defines the minimal contract that Application layer Use Cases require from the database context.
///
/// Keeping this interface in the Application layer ensures that handlers never import
/// a concrete EF Core type, maintaining Clean Architecture dependency direction:
///     Application → (abstraction) ← Persistence
///
/// DbSet properties for each Aggregate Root will be added here as Entities are created
/// in subsequent tasks.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Saves all pending changes asynchronously.
    /// Audit fields are populated automatically by <c>ApplicationDbContext</c>.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
