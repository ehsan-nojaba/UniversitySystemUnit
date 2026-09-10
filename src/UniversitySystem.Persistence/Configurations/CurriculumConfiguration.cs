using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

public sealed class CurriculumConfiguration : IEntityTypeConfiguration<Curriculum>
{
    public void Configure(EntityTypeBuilder<Curriculum> builder)
    {
        builder.ToTable("Curriculums");

        builder.HasKey(cu => cu.Id);

        builder.Property(cu => cu.MajorId)
            .IsRequired();

        builder.Property(cu => cu.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cu => cu.Version)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(cu => cu.IsActive)
            .IsRequired();

        builder.HasIndex(cu => cu.MajorId)
            .HasDatabaseName("IX_Curriculums_MajorId");

        builder.HasOne(cu => cu.Major)
            .WithMany(m => m.Curriculums)
            .HasForeignKey(cu => cu.MajorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(cu => cu.CurriculumCourses)
            .WithOne(cc => cc.Curriculum)
            .HasForeignKey(cc => cc.CurriculumId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
