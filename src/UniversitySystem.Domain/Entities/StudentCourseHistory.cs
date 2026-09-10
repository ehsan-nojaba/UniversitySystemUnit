using UniversitySystem.Domain.Common;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Domain.Entities;

public class StudentCourseHistory : BaseAuditableEntity
{
    public long StudentId { get; private set; }
    public long CourseId { get; private set; }
    public long AcademicTermId { get; private set; }
    public decimal? Grade { get; private set; }
    public CourseEnrollmentStatus Status { get; private set; }

    public Student Student { get; private set; } = default!;
    public Course Course { get; private set; } = default!;
    public AcademicTerm AcademicTerm { get; private set; } = default!;

    private StudentCourseHistory() { }

    public StudentCourseHistory(long studentId, long courseId, long academicTermId)
    {
        StudentId = studentId;
        CourseId = courseId;
        AcademicTermId = academicTermId;
        Status = CourseEnrollmentStatus.InProgress;
    }

    public void RecordGrade(decimal grade)
    {
        Grade = grade;
        Status = grade >= 10m ? CourseEnrollmentStatus.Passed : CourseEnrollmentStatus.Failed;
    }

    public void Withdraw()
    {
        Status = CourseEnrollmentStatus.Withdrawn;
        Grade = null;
    }
}
