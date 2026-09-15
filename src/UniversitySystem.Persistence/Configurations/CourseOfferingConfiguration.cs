using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

public sealed class CourseOfferingConfiguration : IEntityTypeConfiguration<CourseOffering>
{
    public void Configure(EntityTypeBuilder<CourseOffering> builder)
    {
        builder.ToTable("CourseOfferings");

        builder.HasKey(co => co.Id);

        builder.Property(co => co.CourseId)
            .IsRequired();

        builder.Property(co => co.AcademicTermId)
            .IsRequired();

        builder.Property(co => co.Capacity)
            .IsRequired();

        builder.Property(co => co.IsActive)
            .IsRequired();

        // No unique constraint on (CourseId, AcademicTermId) intentionally:
        // Multiple sections of the same course in the same term are possible.
        // When SectionNumber is required in future, add it here without rewrite.
        builder.HasIndex(co => new { co.CourseId, co.AcademicTermId })
            .HasDatabaseName("IX_CourseOfferings_CourseId_AcademicTermId");

        builder.HasIndex(co => co.AcademicTermId)
            .HasDatabaseName("IX_CourseOfferings_AcademicTermId");

        builder.HasOne(co => co.Course)
            .WithMany(c => c.Offerings)
            .HasForeignKey(co => co.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(co => co.AcademicTerm)
            .WithMany()
            .HasForeignKey(co => co.AcademicTermId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(co => co.TeachingAssignments)
            .WithOne(ta => ta.CourseOffering)
            .HasForeignKey(ta => ta.CourseOfferingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(co => co.Enrollments)
            .WithOne(e => e.CourseOffering)
            .HasForeignKey(e => e.CourseOfferingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(co => co.Schedules)
            .WithOne(s => s.CourseOffering)
            .HasForeignKey(s => s.CourseOfferingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
