using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل StudentCourseHistory به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class StudentCourseHistoryConfiguration : IEntityTypeConfiguration<StudentCourseHistory>
{
    public void Configure(EntityTypeBuilder<StudentCourseHistory> builder)
    {
        builder.ToTable("StudentCourseHistories");

        builder.HasKey(sch => sch.Id);

        builder.Property(sch => sch.StudentId)
            .IsRequired();

        builder.Property(sch => sch.CourseId)
            .IsRequired();

        builder.Property(sch => sch.AcademicTermId)
            .IsRequired();

        builder.Property(sch => sch.Grade)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        builder.Property(sch => sch.Status)
            .IsRequired()
            .HasConversion<byte>();

        builder.HasIndex(sch => sch.StudentId)
            .HasDatabaseName("IX_StudentCourseHistories_StudentId");

        builder.HasOne(sch => sch.Student)
            .WithMany(s => s.CourseHistory)
            .HasForeignKey(sch => sch.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sch => sch.Course)
            .WithMany()
            .HasForeignKey(sch => sch.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sch => sch.AcademicTerm)
            .WithMany()
            .HasForeignKey(sch => sch.AcademicTermId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
