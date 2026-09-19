using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// ارائه واقعی یک درس در یک ترم، همراه ظرفیت و وضعیت فعالیت؛ تخصیص استاد و زمان کلاس به آن متصل می‌شوند.
/// </summary>
public class CourseOffering : BaseAuditableEntity
{
    public long CourseId { get; set; }
    public long AcademicTermId { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    /// <summary>فقط ارائه نهایی‌شده توسط آموزش به دانشجو برای ثبت‌نام نمایش داده می‌شود.</summary>
    public bool IsFinalized { get; set; }
    public Course Course { get; set; } = default!;
    public AcademicTerm AcademicTerm { get; set; } = default!;
    public ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<CourseOfferingSchedule> Schedules { get; set; } = new List<CourseOfferingSchedule>();
}
