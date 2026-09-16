using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل Enrollment به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments", t =>
        {
            t.HasCheckConstraint(
                "CK_Enrollments_FinalGrade_Range",
                "FinalGrade IS NULL OR (FinalGrade >= 0 AND FinalGrade <= 20)");
        });

        builder.HasKey(e => e.Id);

        builder.Property(e => e.StudentId)
            .IsRequired();

        builder.Property(e => e.CourseOfferingId)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(e => e.EnrolledAt)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(e => e.FinalGrade)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        // A student cannot be enrolled in the same CourseOffering twice
        builder.HasIndex(e => new { e.StudentId, e.CourseOfferingId })
            .IsUnique()
            .HasDatabaseName("UX_Enrollments_StudentId_CourseOfferingId");

        builder.HasIndex(e => e.CourseOfferingId)
            .HasDatabaseName("IX_Enrollments_CourseOfferingId");

        builder.HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // FK to CourseOffering is handled via CourseOfferingConfiguration (Restrict)
        builder.HasOne(e => e.CourseOffering)
            .WithMany(co => co.Enrollments)
            .HasForeignKey(e => e.CourseOfferingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
