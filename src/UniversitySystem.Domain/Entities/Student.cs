using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// پروفایل دانشجو؛ حساب کاربری را به شماره دانشجویی، رشته و سال ورود متصل می‌کند.
/// </summary>
public class Student : BaseAuditableEntity
{
    public long UserId { get; set; }
    public string StudentNumber { get; set; } = default!;
    public long MajorId { get; set; }
    public int EntryYear { get; set; }
    public User User { get; set; } = default!;
    public Major Major { get; set; } = default!;
    public ICollection<StudentCourseHistory> CourseHistory { get; set; } = new List<StudentCourseHistory>();
    public ICollection<StudentPreRegistration> PreRegistrations { get; set; } = new List<StudentPreRegistration>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
