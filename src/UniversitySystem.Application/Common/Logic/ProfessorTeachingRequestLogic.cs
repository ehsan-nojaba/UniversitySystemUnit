using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Application.Common.Exceptions;

namespace UniversitySystem.Application.Common.Logic;
/// <summary>قواعد ساخت و تغییر ProfessorTeachingRequest؛ منطق از مدل داده جدا نگه داشته می‌شود.</summary>
public static class ProfessorTeachingRequestLogic
{
    /// <summary>ساخت مدل با مقادیر اولیه و اعتبارسنجی همان فرایند؛ Entity فقط داده نگه می‌دارد.</summary>
    public static ProfessorTeachingRequest Create(long professorId, long academicTermId)
    {
        var entity = new ProfessorTeachingRequest();
        entity.ProfessorId = professorId;
        entity.AcademicTermId = academicTermId;
        entity.Status = RequestStatus.Draft;
        return entity;
    }

    public static void AddCourse(ProfessorTeachingRequest entity, long courseId, int priority)
    {
        ProfessorTeachingRequestLogic.EnsureEditable(entity);
        if (courseId <= 0 || priority <= 0)
        {
            throw new BusinessException("شناسه درس و اولویت باید معتبر باشند.");
        }

        bool alreadyAdded = entity.Courses.Any(c => c.CourseId == courseId);
        if (!alreadyAdded)
        {
            entity.Courses.Add(ProfessorTeachingRequestCourseLogic.Create(entity.Id, courseId, priority));
        }
    }

    public static void UpdateCoursePriority(ProfessorTeachingRequest entity, long courseId, int priority)
    {
        ProfessorTeachingRequestLogic.EnsureEditable(entity);
        var course = entity.Courses.Single(c => c.CourseId == courseId);
        ProfessorTeachingRequestCourseLogic.UpdatePriority(course, priority);
    }

    public static void ReplaceAvailability(ProfessorTeachingRequest entity, IEnumerable<(DayOfWeek Day, TimeOnly Start, TimeOnly End)> intervals)
    {
        ProfessorTeachingRequestLogic.EnsureEditable(entity);
        var values = intervals.ToList();
        if (values.Any(v => !Enum.IsDefined(v.Day) || v.Start >= v.End) || values.Select((v, i) => values.Skip(i + 1).Any(n => v.Day == n.Day && v.Start < n.End && n.Start < v.End)).Any(x => x))
        {
            throw new BusinessException("بازه‌های زمانی باید معتبر باشند و نباید هم‌پوشانی داشته باشند.");
        }

        entity.Availabilities.Clear();
        foreach (var value in values)
            ProfessorTeachingRequestLogic.AddAvailability(entity, value.Day, value.Start, value.End);
    }

    public static void RemoveCourse(ProfessorTeachingRequest entity, long courseId)
    {
        ProfessorTeachingRequestLogic.EnsureEditable(entity);
        var course = entity.Courses.FirstOrDefault(c => c.CourseId == courseId);
        if (course is not null)
        {
            entity.Courses.Remove(course);
            foreach (var availability in entity.Availabilities.Where(a => a.CourseId == courseId).ToList())
        {
            entity.Availabilities.Remove(availability);
        }
        }
    }

    public static void ReplaceCourseAvailability(ProfessorTeachingRequest entity, IEnumerable<(long CourseId, DayOfWeek Day, TimeOnly Start, TimeOnly End)> intervals)
    {
        ProfessorTeachingRequestLogic.EnsureEditable(entity);
        var values = intervals.ToList();
        if (values.Any(v => !entity.Courses.Any(c => c.CourseId == v.CourseId) || !Enum.IsDefined(v.Day) || v.Start >= v.End))
        {
            throw new BusinessException("درس و بازه زمانی پیشنهادی معتبر نیست.");
        }

        if (values.Select((v, i) => values.Skip(i + 1).Any(n => n.CourseId == v.CourseId && n.Day == v.Day && n.Start < v.End && v.Start < n.End)).Any(x => x))
        {
            throw new BusinessException("بازه‌های پیشنهادی یک درس نباید هم‌پوشانی داشته باشند.");
        }

        entity.Availabilities.Clear();
        foreach (var value in values)
        {
            entity.Availabilities.Add(ProfessorAvailabilityLogic.Create(entity.Id, value.Day, value.Start, value.End, value.CourseId));
        }
    }

    public static void AddAvailability(ProfessorTeachingRequest entity, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        ProfessorTeachingRequestLogic.EnsureEditable(entity);
        if (!Enum.IsDefined(dayOfWeek))
        {
            throw new BusinessException("روز هفته نامعتبر است.");
        }

        if (endTime <= startTime)
        {
            throw new BusinessException("زمان پایان باید بعد از زمان شروع باشد.");
        }

        if (entity.Availabilities.Any(a => a.DayOfWeek == dayOfWeek && a.StartTime < endTime && startTime < a.EndTime))
        {
            throw new BusinessException("بازه‌های زمانی نمی‌توانند هم‌پوشانی داشته باشند.");
        }

        bool alreadyExists = entity.Availabilities.Any(a => a.DayOfWeek == dayOfWeek && a.StartTime == startTime && a.EndTime == endTime);
        if (!alreadyExists)
        {
            entity.Availabilities.Add(ProfessorAvailabilityLogic.Create(entity.Id, dayOfWeek, startTime, endTime));
        }
    }

    public static void RemoveAvailability(ProfessorTeachingRequest entity, long availabilityId)
    {
        ProfessorTeachingRequestLogic.EnsureEditable(entity);
        var availability = entity.Availabilities.FirstOrDefault(a => a.Id == availabilityId);
        if (availability is not null)
        {
            entity.Availabilities.Remove(availability);
        }
    }

    public static void Submit(ProfessorTeachingRequest entity, DateTime utcNow)
    {
        ProfessorTeachingRequestLogic.EnsureEditable(entity);
        if (entity.Courses.Count == 0)
        {
            throw new BusinessException("حداقل یک درس باید انتخاب شود.");
        }

        entity.Status = RequestStatus.Submitted;
        entity.SubmittedAt = utcNow;
    }

    public static void Cancel(ProfessorTeachingRequest entity)
    {
        if (entity.Status == RequestStatus.Cancelled)
        {
            return;
        }

        entity.Status = RequestStatus.Cancelled;
    }

    private static void EnsureEditable(ProfessorTeachingRequest entity)
    {
        if (entity.Status != RequestStatus.Draft)
        {
            throw new BusinessException("درخواست تدریس فقط در وضعیت پیش‌نویس قابل ویرایش است.");
        }
    }
}
