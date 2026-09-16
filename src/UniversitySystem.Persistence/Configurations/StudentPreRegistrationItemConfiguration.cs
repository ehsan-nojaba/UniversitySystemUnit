using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل StudentPreRegistrationItem به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class StudentPreRegistrationItemConfiguration : IEntityTypeConfiguration<StudentPreRegistrationItem>
{
    public void Configure(EntityTypeBuilder<StudentPreRegistrationItem> builder)
    {
        builder.ToTable("StudentPreRegistrationItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.StudentPreRegistrationId)
            .IsRequired();

        builder.Property(i => i.CourseId)
            .IsRequired();

        builder.Property(i => i.Priority)
            .IsRequired();

        builder.HasIndex(i => new { i.StudentPreRegistrationId, i.CourseId })
            .IsUnique()
            .HasDatabaseName("UX_StudentPreRegistrationItems_RegistrationId_CourseId");

        // FK to StudentPreRegistration handled via StudentPreRegistrationConfiguration (Cascade)
        builder.HasOne(i => i.StudentPreRegistration)
            .WithMany(spr => spr.Items)
            .HasForeignKey(i => i.StudentPreRegistrationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Course)
            .WithMany()
            .HasForeignKey(i => i.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
