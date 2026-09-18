using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Configurations;

/// <summary>نگاشت رابطه یکتای استاد و درس مجاز، بدون حذف آبشاری اطلاعات آموزشی.</summary>
public sealed class ProfessorCourseConfiguration : IEntityTypeConfiguration<ProfessorCourse>
{
    public void Configure(EntityTypeBuilder<ProfessorCourse> builder)
    {
        builder.ToTable("ProfessorCourses");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.ProfessorId, x.CourseId }).IsUnique();
        builder.HasOne(x => x.Professor).WithMany().HasForeignKey(x => x.ProfessorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Course).WithMany().HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Restrict);
    }
}
