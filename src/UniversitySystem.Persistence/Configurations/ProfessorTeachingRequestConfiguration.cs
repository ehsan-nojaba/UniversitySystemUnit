using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Persistence.Configurations;

public sealed class ProfessorTeachingRequestConfiguration : IEntityTypeConfiguration<ProfessorTeachingRequest>
{
    public void Configure(EntityTypeBuilder<ProfessorTeachingRequest> builder)
    {
        builder.ToTable("ProfessorTeachingRequests");

        builder.HasKey(ptr => ptr.Id);

        builder.Property(ptr => ptr.ProfessorId)
            .IsRequired();

        builder.Property(ptr => ptr.AcademicTermId)
            .IsRequired();

        builder.Property(ptr => ptr.Status)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(ptr => ptr.SubmittedAt)
            .HasColumnType("datetime2")
            .IsRequired(false);

        // A professor can only have one teaching request per term
        builder.HasIndex(ptr => new { ptr.ProfessorId, ptr.AcademicTermId })
            .IsUnique()
            .HasDatabaseName("UX_ProfessorTeachingRequests_ProfessorId_AcademicTermId");

        builder.HasOne(ptr => ptr.Professor)
            .WithMany(p => p.TeachingRequests)
            .HasForeignKey(ptr => ptr.ProfessorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ptr => ptr.AcademicTerm)
            .WithMany()
            .HasForeignKey(ptr => ptr.AcademicTermId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ptr => ptr.Courses)
            .WithOne(c => c.ProfessorTeachingRequest)
            .HasForeignKey(c => c.ProfessorTeachingRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(ptr => ptr.Availabilities)
            .WithOne(a => a.ProfessorTeachingRequest)
            .HasForeignKey(a => a.ProfessorTeachingRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
