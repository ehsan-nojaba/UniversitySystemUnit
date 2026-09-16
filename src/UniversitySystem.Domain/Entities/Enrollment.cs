using UniversitySystem.Domain.Common;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Domain.Entities;

/// <summary>
/// ثبت‌نام قطعی دانشجو در یک ارائه؛ زمان ثبت‌نام، وضعیت و نمره نهایی را نگه می‌دارد.
/// </summary>
public class Enrollment : BaseAuditableEntity
{
    public long StudentId { get; private set; }
    public long CourseOfferingId { get; private set; }
    public EnrollmentStatus Status { get; private set; }
    public DateTime EnrolledAt { get; private set; }
    public decimal? FinalGrade { get; private set; }

    public Student Student { get; private set; } = default!;
    public CourseOffering CourseOffering { get; private set; } = default!;

    private Enrollment() { }

    public Enrollment(long studentId, long courseOfferingId, DateTime enrolledAt)
    {
        if (studentId <= 0)
            throw new ArgumentException("Enrollment must have a valid Student.", nameof(studentId));

        if (courseOfferingId <= 0)
            throw new ArgumentException("Enrollment must have a valid CourseOffering.", nameof(courseOfferingId));

        StudentId = studentId;
        CourseOfferingId = courseOfferingId;
        EnrolledAt = enrolledAt;
        Status = EnrollmentStatus.Enrolled;
    }

    public void RecordGrade(decimal grade)
    {
        if (grade < 0m || grade > 20m)
            throw new ArgumentOutOfRangeException(nameof(grade), "Final grade must be between 0 and 20.");

        FinalGrade = grade;
        Status = grade >= 10m ? EnrollmentStatus.Completed : EnrollmentStatus.Failed;
    }

    public void Withdraw()
    {
        if (Status != EnrollmentStatus.Enrolled)
            throw new InvalidOperationException("Only active enrollments can be withdrawn.");

        Status = EnrollmentStatus.Withdrawn;
        FinalGrade = null;
    }
}
