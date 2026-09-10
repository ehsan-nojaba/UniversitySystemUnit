using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

public sealed class ProfessorAvailabilityConfiguration : IEntityTypeConfiguration<ProfessorAvailability>
{
    public void Configure(EntityTypeBuilder<ProfessorAvailability> builder)
    {
        builder.ToTable("ProfessorAvailabilities");

        builder.HasKey(pa => pa.Id);

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
