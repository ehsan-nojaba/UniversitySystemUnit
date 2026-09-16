using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل AcademicTerm به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class AcademicTermConfiguration : IEntityTypeConfiguration<AcademicTerm>
{
    public void Configure(EntityTypeBuilder<AcademicTerm> builder)
    {
        builder.ToTable("AcademicTerms");

        builder.HasKey(at => at.Id);

        builder.Property(at => at.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(at => at.Code)
            .IsUnique()
            .HasDatabaseName("UX_AcademicTerms_Code");

        builder.Property(at => at.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(at => at.StartDate)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(at => at.EndDate)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(at => at.IsActive)
            .IsRequired();
    }
}
