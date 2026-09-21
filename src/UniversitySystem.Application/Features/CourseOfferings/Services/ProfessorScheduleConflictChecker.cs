using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;

namespace UniversitySystem.Application.Features.CourseOfferings.Services;
/// <summary>
/// قانون مشترک بررسی تداخل استاد و پوشش زمان کلاس توسط زمان‌های آزاد؛ هنگام ذخیره برنامه و تخصیص استاد استفاده می‌شود.
/// </summary>
public sealed class ProfessorScheduleConflictChecker(IProfessorScheduleRepository repository) : IProfessorScheduleConflictChecker
{
    public Task CheckConflictsAsync(long courseOfferingId, ICollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default)
    {
        return CheckAsync(courseOfferingId, slots, null, cancellationToken);
    }
    public Task CheckProfessorAssignmentAsync(long courseOfferingId, long professorId, ICollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default)
    {
        return CheckAsync(courseOfferingId, slots, professorId, cancellationToken);
    }
    private async Task CheckAsync(long offeringId, ICollection<CourseOfferingScheduleSlotDto> slots, long? professorId, CancellationToken cancellationToken)
    {
        if (slots.Count == 0)
        {
            return;
        }

        var professors = await repository.GetDataAsync(offeringId, professorId, cancellationToken);
        foreach (var professor in professors)
            foreach (var slot in slots)
            {
                if (professor.OtherSchedules.Any(s => s.DayOfWeek == slot.DayOfWeek && s.StartTime < slot.EndTime && slot.StartTime < s.EndTime))
                {
                    throw new BusinessException($"زمان کلاس با برنامه دیگر استاد {professor.FullName} تداخل دارد.");
                }

                if (!professor.Availability.Any(a => a.DayOfWeek == slot.DayOfWeek && a.StartTime <= slot.StartTime && slot.EndTime <= a.EndTime))
                {
                    throw new BusinessException($"زمان کلاس باید داخل پیشنهاد زمانی استاد {professor.FullName} برای همین درس باشد.");
                }
            }
    }
}
