namespace UniversitySystem.Domain.Common;

/// <summary>
/// Extends <see cref="BaseEntity"/> with a full audit trail.
///
/// All four audit fields are set automatically by <c>ApplicationDbContext.SaveChangesAsync</c>
/// via <c>IDateTimeProvider</c> and <c>ICurrentUserService</c>.
/// Business code must never set these fields manually.
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity
{
    /// <summary>UTC timestamp of when the entity was first persisted.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Identifier of the user who created the entity. Null for system-generated records.</summary>
    public string? CreatedBy { get; set; }

    /// <summary>UTC timestamp of the most recent update. Null until the entity is modified after creation.</summary>
    public DateTime? LastModifiedAt { get; set; }

    /// <summary>Identifier of the user who last modified the entity. Null until the entity is modified after creation.</summary>
    public string? LastModifiedBy { get; set; }
}
