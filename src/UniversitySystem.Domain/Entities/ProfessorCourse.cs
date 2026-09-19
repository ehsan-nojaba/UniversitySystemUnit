using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>ارتباط پایه استاد با درس‌هایی که آموزش اجازه تدریس آن‌ها را داده است؛ با تخصیص نهایی به کلاس فرق دارد.</summary>
public sealed class ProfessorCourse : BaseAuditableEntity
{
    public long ProfessorId { get; set; }
    public long CourseId { get; set; }
    public Professor Professor { get; set; } = default!;
    public Course Course { get; set; } = default!;
}
