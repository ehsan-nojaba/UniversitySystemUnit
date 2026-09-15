using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.CourseOfferings.Services;

/// <summary>
/// پیاده‌سازی سرویس بررسی تداخل زمانی و تطابق بازه حضور اساتید.
/// </summary>
public sealed class ProfessorScheduleConflictChecker(IApplicationDbContext context) : IProfessorScheduleConflictChecker
{
    public async Task CheckConflictsAsync(
        long courseOfferingId,
        IReadOnlyCollection<CourseOfferingScheduleSlotDto> slots,
        CancellationToken cancellationToken = default)
    {
        var currentOffering = await context.CourseOfferings
            .AsNoTracking()
            .FirstOrDefaultAsync(co => co.Id == courseOfferingId, cancellationToken);

        if (currentOffering is null)
            throw new NotFoundException(nameof(CourseOffering), courseOfferingId);

        if (slots.Count == 0)
            return;

        // 1. Professorهای Assigned به Offering را پیدا کن
        var assignedProfessors = await (
            from ta in context.TeachingAssignments.AsNoTracking()
            join p in context.Professors.AsNoTracking() on ta.ProfessorId equals p.Id
            join u in context.Users.AsNoTracking() on p.UserId equals u.Id
            where ta.CourseOfferingId == courseOfferingId
            select new
            {
                ta.ProfessorId,
                FullName = u.FirstName + " " + u.LastName
            }
        ).ToListAsync(cancellationToken);

        if (assignedProfessors.Count == 0)
            return;

        var profIds = assignedProfessors.Select(p => p.ProfessorId).Distinct().ToList();

        // 2. Assignmentهای دیگر همان Professorها را در همان ترم پیدا کن
        var otherOfferingSchedules = await (
            from ta in context.TeachingAssignments.AsNoTracking()
            join co in context.CourseOfferings.AsNoTracking() on ta.CourseOfferingId equals co.Id
            join c in context.Courses.AsNoTracking() on co.CourseId equals c.Id
            join s in context.CourseOfferingSchedules.AsNoTracking() on co.Id equals s.CourseOfferingId
            where profIds.Contains(ta.ProfessorId)
               && ta.CourseOfferingId != courseOfferingId
               && co.AcademicTermId == currentOffering.AcademicTermId
            select new
            {
                ta.ProfessorId,
                CourseCode = c.Code,
                CourseTitle = c.Title,
                s.DayOfWeek,
                s.StartTime,
                s.EndTime
            }
        ).ToListAsync(cancellationToken);

        // 3. Scheduleهای همان روز را بررسی کن و در صورت تداخل، BusinessException برگردان
        foreach (var prof in assignedProfessors)
        {
            var profOtherSchedules = otherOfferingSchedules.Where(s => s.ProfessorId == prof.ProfessorId).ToList();

            foreach (var newSlot in slots)
            {
                foreach (var existing in profOtherSchedules)
                {
                    if (existing.DayOfWeek == newSlot.DayOfWeek)
                    {
                        // Overlap: existing.StartTime < new.EndTime AND new.StartTime < existing.EndTime
                        if (existing.StartTime < newSlot.EndTime && newSlot.StartTime < existing.EndTime)
                        {
                            var dayName = GetPersianDayName(newSlot.DayOfWeek);
                            throw new BusinessException(
                                $"تداخل زمانی برای استاد {prof.FullName}: درس با ارائه درس «{existing.CourseTitle}» ({existing.CourseCode}) در روز {dayName} و در بازه زمانی {existing.StartTime:HH\\:mm} تا {existing.EndTime:HH\\:mm} تداخل دارد.");
                        }
                    }
                }
            }
        }

        // 4. بررسی Availability استاد در همان ترم تحصیلی
        var teachingRequests = await (
            from r in context.ProfessorTeachingRequests.AsNoTracking()
            where profIds.Contains(r.ProfessorId)
               && r.AcademicTermId == currentOffering.AcademicTermId
               && r.Status == RequestStatus.Submitted
            select new
            {
                r.ProfessorId,
                Availabilities = r.Availabilities.Select(a => new
                {
                    a.DayOfWeek,
                    a.StartTime,
                    a.EndTime
                }).ToList()
            }
        ).ToListAsync(cancellationToken);

        foreach (var prof in assignedProfessors)
        {
            var profReq = teachingRequests.FirstOrDefault(r => r.ProfessorId == prof.ProfessorId);
            if (profReq is not null && profReq.Availabilities.Count > 0)
            {
                foreach (var newSlot in slots)
                {
                    bool isCovered = profReq.Availabilities.Any(a =>
                        a.DayOfWeek == newSlot.DayOfWeek &&
                        a.StartTime <= newSlot.StartTime &&
                        newSlot.EndTime <= a.EndTime);

                    if (!isCovered)
                    {
                        var dayName = GetPersianDayName(newSlot.DayOfWeek);
                        throw new ValidationException([
                            new ValidationFailure(
                                "Slots",
                                $"زمان‌بندی انتخاب‌شده برای روز {dayName} ({newSlot.StartTime:HH\\:mm} تا {newSlot.EndTime:HH\\:mm}) خارج از بازه زمانی اعلام‌شده حضور استاد {prof.FullName} در این ترم تحصیلی است.")
                        ]);
                    }
                }
            }
        }
    }

    private static string GetPersianDayName(DayOfWeek day) => day switch
    {
        DayOfWeek.Saturday => "شنبه",
        DayOfWeek.Sunday => "یکشنبه",
        DayOfWeek.Monday => "دوشنبه",
        DayOfWeek.Tuesday => "سه‌شنبه",
        DayOfWeek.Wednesday => "چهارشنبه",
        DayOfWeek.Thursday => "پنج‌شنبه",
        DayOfWeek.Friday => "جمعه",
        _ => day.ToString()
    };
}
