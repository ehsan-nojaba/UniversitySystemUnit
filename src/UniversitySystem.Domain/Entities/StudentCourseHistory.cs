using UniversitySystem.Domain.Common;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// سابقه درس دانشجو شامل ترم، نمره و وضعیت قبولی؛ در بررسی درس‌های پاس‌شده و پیش‌نیازها استفاده می‌شود.
/// </summary>
public class StudentCourseHistory : BaseAuditableEntity
{
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public long AcademicTermId { get; set; }
    public decimal? Grade { get; set; }
    public CourseEnrollmentStatus Status { get; set; }
    public Student Student { get; set; } = default!;
    public Course Course { get; set; } = default!;
    public AcademicTerm AcademicTerm { get; set; } = default!;
}
