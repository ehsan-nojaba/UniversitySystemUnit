using UniversitySystem.Domain.Common;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Domain.Entities;
/// <summary>
/// ثبت‌نام قطعی دانشجو در یک ارائه؛ زمان ثبت‌نام، وضعیت و نمره نهایی را نگه می‌دارد.
/// </summary>
public class Enrollment : BaseAuditableEntity
{
    public long StudentId { get; set; }
    public long CourseOfferingId { get; set; }
    public EnrollmentStatus Status { get; set; }
    public DateTime EnrolledAt { get; set; }
    public decimal? FinalGrade { get; set; }
    public Student Student { get; set; } = default!;
    public CourseOffering CourseOffering { get; set; } = default!;
}
