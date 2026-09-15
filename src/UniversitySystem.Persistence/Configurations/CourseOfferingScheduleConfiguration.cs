using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

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
