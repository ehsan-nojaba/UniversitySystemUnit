using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>
/// تنظیم نگاشت مدل ProfessorAvailability به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد.
/// </summary>
public sealed class ProfessorAvailabilityConfiguration : IEntityTypeConfiguration<ProfessorAvailability>
{
    public void Configure(EntityTypeBuilder<ProfessorAvailability> builder)
    {
        builder.ToTable("ProfessorAvailabilities");

        builder.HasKey(pa => pa.Id);
        builder.HasOne<Course>().WithMany().HasForeignKey(pa => pa.CourseId).OnDelete(DeleteBehavior.Restrict);

        builder.Property(pa => pa.ProfessorTeachingRequestId)
            .IsRequired();

        builder.Property(pa => pa.DayOfWeek)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(pa => pa.StartTime)
            .IsRequired();

        builder.Property(pa => pa.EndTime)
            .IsRequired();

        // FK to ProfessorTeachingRequest handled via ProfessorTeachingRequestConfiguration (Cascade)
        builder.HasOne(pa => pa.ProfessorTeachingRequest)
            .WithMany(ptr => ptr.Availabilities)
            .HasForeignKey(pa => pa.ProfessorTeachingRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
