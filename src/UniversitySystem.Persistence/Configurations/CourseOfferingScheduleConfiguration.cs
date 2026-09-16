using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل CourseOfferingSchedule به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class CourseOfferingScheduleConfiguration : IEntityTypeConfiguration<CourseOfferingSchedule>
{
    public void Configure(EntityTypeBuilder<CourseOfferingSchedule> builder)
    {
        builder.ToTable("CourseOfferingSchedules");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.CourseOfferingId)
            .IsRequired();

        builder.Property(s => s.DayOfWeek)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(s => s.StartTime)
            .IsRequired()
            .HasColumnType("time");

        builder.Property(s => s.EndTime)
            .IsRequired()
            .HasColumnType("time");

        builder.HasIndex(s => s.CourseOfferingId)
            .HasDatabaseName("IX_CourseOfferingSchedules_CourseOfferingId");

        builder.HasOne(s => s.CourseOffering)
            .WithMany(co => co.Schedules)
            .HasForeignKey(s => s.CourseOfferingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
