using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر StudentCourseHistory؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class StudentCourseHistoryLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static StudentCourseHistory Create(long studentId, long courseId, long academicTermId)
    {
        var entity = new StudentCourseHistory();
        entity.StudentId = studentId;
        entity.CourseId = courseId;
        entity.AcademicTermId = academicTermId;
        entity.Status = CourseEnrollmentStatus.InProgress;
        return entity;
    }

    public static void RecordGrade(StudentCourseHistory entity, decimal grade)
    {
        entity.Grade = grade;
        entity.Status = grade >= 10m ? CourseEnrollmentStatus.Passed : CourseEnrollmentStatus.Failed;
    }

    public static void Withdraw(StudentCourseHistory entity)
    {
        entity.Status = CourseEnrollmentStatus.Withdrawn;
        entity.Grade = null;
    }
}
