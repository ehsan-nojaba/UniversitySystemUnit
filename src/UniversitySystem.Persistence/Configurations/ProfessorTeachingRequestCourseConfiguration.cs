using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل ProfessorTeachingRequestCourse به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class ProfessorTeachingRequestCourseConfiguration : IEntityTypeConfiguration<ProfessorTeachingRequestCourse>
{
    public void Configure(EntityTypeBuilder<ProfessorTeachingRequestCourse> builder)
    {
        builder.ToTable("ProfessorTeachingRequestCourses");

        builder.HasKey(ptrc => ptrc.Id);

        builder.Property(ptrc => ptrc.ProfessorTeachingRequestId)
            .IsRequired();

        builder.Property(ptrc => ptrc.CourseId)
            .IsRequired();

        builder.Property(ptrc => ptrc.Priority)
            .IsRequired();

        builder.HasIndex(ptrc => new { ptrc.ProfessorTeachingRequestId, ptrc.CourseId })
            .IsUnique()
            .HasDatabaseName("UX_ProfessorTeachingRequestCourses_RequestId_CourseId");

        // FK to ProfessorTeachingRequest handled via ProfessorTeachingRequestConfiguration (Cascade)
        builder.HasOne(ptrc => ptrc.ProfessorTeachingRequest)
            .WithMany(ptr => ptr.Courses)
            .HasForeignKey(ptrc => ptrc.ProfessorTeachingRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ptrc => ptrc.Course)
            .WithMany()
            .HasForeignKey(ptrc => ptrc.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
