using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل Major به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class MajorConfiguration : IEntityTypeConfiguration<Major>
{
    public void Configure(EntityTypeBuilder<Major> builder)
    {
        builder.ToTable("Majors");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.DepartmentId)
            .IsRequired();

        builder.Property(m => m.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.IsActive)
            .IsRequired();

        builder.HasIndex(m => m.DepartmentId)
            .HasDatabaseName("IX_Majors_DepartmentId");

        // FK to Department already configured from DepartmentConfiguration
        // but we need to ensure it's Restrict here too for EF tracking
        builder.HasOne(m => m.Department)
            .WithMany(d => d.Majors)
            .HasForeignKey(m => m.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
