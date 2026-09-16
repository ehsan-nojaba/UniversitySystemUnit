using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل Department به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.FacultyId)
            .IsRequired();

        builder.Property(d => d.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.IsActive)
            .IsRequired();

        // Code is unique within a Faculty (not globally)
        builder.HasIndex(d => new { d.FacultyId, d.Code })
            .IsUnique()
            .HasDatabaseName("UX_Departments_FacultyId_Code");

        builder.HasIndex(d => d.FacultyId)
            .HasDatabaseName("IX_Departments_FacultyId");

        builder.HasMany(d => d.Majors)
            .WithOne(m => m.Department)
            .HasForeignKey(m => m.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
