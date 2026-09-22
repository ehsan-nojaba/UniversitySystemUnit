using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر Enrollment؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class EnrollmentLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static Enrollment Create(long studentId, long courseOfferingId, DateTime enrolledAt)
    {
        var entity = new Enrollment();
        if (studentId <= 0)
        {
            throw new ArgumentException("شناسه دانشجو برای ثبت‌نام باید معتبر باشد.", nameof(studentId));
        }

        if (courseOfferingId <= 0)
        {
            throw new ArgumentException("شناسه ارائه درس برای ثبت‌نام باید معتبر باشد.", nameof(courseOfferingId));
        }

        entity.StudentId = studentId;
        entity.CourseOfferingId = courseOfferingId;
        entity.EnrolledAt = enrolledAt;
        entity.Status = EnrollmentStatus.Enrolled;
        return entity;
    }

    public static void RecordGrade(Enrollment entity, decimal grade)
    {
        if (grade < 0m || grade > 20m)
        {
            throw new ArgumentOutOfRangeException(nameof(grade), "نمره نهایی باید بین صفر تا ۲۰ باشد.");
        }

        entity.FinalGrade = grade;
        entity.Status = grade >= 10m ? EnrollmentStatus.Completed : EnrollmentStatus.Failed;
    }

    public static void Withdraw(Enrollment entity)
    {
        if (entity.Status != EnrollmentStatus.Enrolled)
        {
            throw new InvalidOperationException("فقط ثبت‌نام‌های فعال قابل حذف هستند.");
        }

        entity.Status = EnrollmentStatus.Withdrawn;
        entity.FinalGrade = null;
    }
}
