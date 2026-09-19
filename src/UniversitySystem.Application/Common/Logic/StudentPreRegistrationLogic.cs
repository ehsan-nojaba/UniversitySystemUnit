using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر StudentPreRegistration؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class StudentPreRegistrationLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static StudentPreRegistration Create(long studentId, long academicTermId)
    {
        var entity = new StudentPreRegistration();
        entity.StudentId = studentId;
        entity.AcademicTermId = academicTermId;
        entity.Status = RequestStatus.Draft;
        return entity;
    }

    public static void AddCourse(StudentPreRegistration entity, long courseId, int priority)
    {
        StudentPreRegistrationLogic.EnsureEditable(entity);
        bool alreadyAdded = entity.Items.Any(i => i.CourseId == courseId);
        if (!alreadyAdded)
        {
            entity.Items.Add(StudentPreRegistrationItemLogic.Create(entity.Id, courseId, priority));
        }
    }

    public static void RemoveCourse(StudentPreRegistration entity, long courseId)
    {
        StudentPreRegistrationLogic.EnsureEditable(entity);
        var item = entity.Items.FirstOrDefault(i => i.CourseId == courseId);
        if (item is not null)
        {
            entity.Items.Remove(item);
        }
    }

    public static void UpdateCoursePriority(StudentPreRegistration entity, long courseId, int priority)
    {
        StudentPreRegistrationLogic.EnsureEditable(entity);
        var item = entity.Items.FirstOrDefault(i => i.CourseId == courseId);
        if (item is not null)
        {
            StudentPreRegistrationItemLogic.UpdatePriority(item, priority);
        }
    }

    public static void Submit(StudentPreRegistration entity, DateTime utcNow)
    {
        StudentPreRegistrationLogic.EnsureEditable(entity);
        if (entity.Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot submit a pre-registration with no courses selected.");
        }

        entity.Status = RequestStatus.Submitted;
        entity.SubmittedAt = utcNow;
    }

    public static void Cancel(StudentPreRegistration entity)
    {
        if (entity.Status == RequestStatus.Cancelled)
        {
            return;
        }

        entity.Status = RequestStatus.Cancelled;
    }

    private static void EnsureEditable(StudentPreRegistration entity)
    {
        if (entity.Status != RequestStatus.Draft)
        {
            throw new InvalidOperationException("Pre-registration can only be modified while in Draft status.");
        }
    }
}
