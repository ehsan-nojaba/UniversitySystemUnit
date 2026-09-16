using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل CurriculumCourse به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class CurriculumCourseConfiguration : IEntityTypeConfiguration<CurriculumCourse>
{
    public void Configure(EntityTypeBuilder<CurriculumCourse> builder)
    {
        builder.ToTable("CurriculumCourses");

        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.CurriculumId)
            .IsRequired();

        builder.Property(cc => cc.CourseId)
            .IsRequired();

        builder.Property(cc => cc.RecommendedTerm)
            .IsRequired();

        builder.Property(cc => cc.IsRequired)
            .IsRequired();

        builder.HasIndex(cc => new { cc.CurriculumId, cc.CourseId })
            .IsUnique()
            .HasDatabaseName("UX_CurriculumCourses_CurriculumId_CourseId");

        // FK to Curriculum is handled via CurriculumConfiguration (Cascade)
        builder.HasOne(cc => cc.Curriculum)
            .WithMany(cu => cu.CurriculumCourses)
            .HasForeignKey(cc => cc.CurriculumId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cc => cc.Course)
            .WithMany()
            .HasForeignKey(cc => cc.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
