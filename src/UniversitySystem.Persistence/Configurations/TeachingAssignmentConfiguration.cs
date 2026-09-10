using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

public sealed class TeachingAssignmentConfiguration : IEntityTypeConfiguration<TeachingAssignment>
{
    public void Configure(EntityTypeBuilder<TeachingAssignment> builder)
    {
        builder.ToTable("TeachingAssignments");

        builder.HasKey(ta => ta.Id);

        builder.Property(ta => ta.CourseOfferingId)
            .IsRequired();

        builder.Property(ta => ta.ProfessorId)
            .IsRequired();

        builder.Property(ta => ta.AssignedAt)
            .IsRequired()
            .HasColumnType("datetime2");

        // A professor cannot be assigned to the same offering twice
        builder.HasIndex(ta => new { ta.CourseOfferingId, ta.ProfessorId })
            .IsUnique()
            .HasDatabaseName("UX_TeachingAssignments_OfferingId_ProfessorId");

        builder.HasIndex(ta => ta.ProfessorId)
            .HasDatabaseName("IX_TeachingAssignments_ProfessorId");

        // FK to CourseOffering is handled via CourseOfferingConfiguration (Cascade)
        builder.HasOne(ta => ta.CourseOffering)
            .WithMany(co => co.TeachingAssignments)
            .HasForeignKey(ta => ta.CourseOfferingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ta => ta.Professor)
            .WithMany(p => p.TeachingAssignments)
            .HasForeignKey(ta => ta.ProfessorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
