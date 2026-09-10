using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Persistence.Configurations;

public sealed class StudentPreRegistrationConfiguration : IEntityTypeConfiguration<StudentPreRegistration>
{
    public void Configure(EntityTypeBuilder<StudentPreRegistration> builder)
    {
        builder.ToTable("StudentPreRegistrations");

        builder.HasKey(spr => spr.Id);

        builder.Property(spr => spr.StudentId)
            .IsRequired();

        builder.Property(spr => spr.AcademicTermId)
            .IsRequired();

        builder.Property(spr => spr.Status)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(spr => spr.SubmittedAt)
            .HasColumnType("datetime2")
            .IsRequired(false);

        // A student can only have one pre-registration per term
        builder.HasIndex(spr => new { spr.StudentId, spr.AcademicTermId })
            .IsUnique()
            .HasDatabaseName("UX_StudentPreRegistrations_StudentId_AcademicTermId");

        builder.HasOne(spr => spr.Student)
            .WithMany(s => s.PreRegistrations)
            .HasForeignKey(spr => spr.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(spr => spr.AcademicTerm)
            .WithMany()
            .HasForeignKey(spr => spr.AcademicTermId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(spr => spr.Items)
            .WithOne(i => i.StudentPreRegistration)
            .HasForeignKey(i => i.StudentPreRegistrationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
