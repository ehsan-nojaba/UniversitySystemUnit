using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل CoursePrerequisite به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class CoursePrerequisiteConfiguration : IEntityTypeConfiguration<CoursePrerequisite>
{
    public void Configure(EntityTypeBuilder<CoursePrerequisite> builder)
    {
        builder.ToTable("CoursePrerequisites");

        builder.HasKey(cp => cp.Id);

        builder.HasIndex(cp => new { cp.CourseId, cp.PrerequisiteCourseId })
            .IsUnique()
            .HasDatabaseName("UX_CoursePrerequisites_CourseId_PrerequisiteId");

        builder.HasOne(cp => cp.Course)
            .WithMany(c => c.Prerequisites)
            .HasForeignKey(cp => cp.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cp => cp.PrerequisiteCourse)
            .WithMany()
            .HasForeignKey(cp => cp.PrerequisiteCourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
