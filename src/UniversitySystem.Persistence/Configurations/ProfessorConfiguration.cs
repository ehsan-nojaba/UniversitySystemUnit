using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل Professor به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class ProfessorConfiguration : IEntityTypeConfiguration<Professor>
{
    public void Configure(EntityTypeBuilder<Professor> builder)
    {
        builder.ToTable("Professors");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.HasIndex(p => p.UserId)
            .IsUnique()
            .HasDatabaseName("UX_Professors_UserId");

        builder.Property(p => p.PersonnelCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(p => p.PersonnelCode)
            .IsUnique()
            .HasDatabaseName("UX_Professors_PersonnelCode");

        builder.HasOne(p => p.User)
            .WithOne(u => u.Professor)
            .HasForeignKey<Professor>(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
