using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل Faculty به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
{
    public void Configure(EntityTypeBuilder<Faculty> builder)
    {
        builder.ToTable("Faculties");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.IsActive)
            .IsRequired();

        builder.HasIndex(f => f.Code)
            .IsUnique()
            .HasDatabaseName("UX_Faculties_Code");

        builder.HasMany(f => f.Departments)
            .WithOne(d => d.Faculty)
            .HasForeignKey(d => d.FacultyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
