using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

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
