using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;

namespace UniversitySystem.Application.Features.CourseOfferings.Services;
/// <summary>
/// قانون مشترک بررسی تداخل استاد و پوشش زمان کلاس توسط زمان‌های آزاد؛ هنگام ذخیره برنامه و تخصیص استاد استفاده می‌شود.
/// </summary>
public sealed class ProfessorScheduleConflictChecker(IProfessorScheduleRepository repository) : IProfessorScheduleConflictChecker
{
    public Task CheckConflictsAsync(long courseOfferingId, IReadOnlyCollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default)
        => CheckAsync(courseOfferingId, slots, null, cancellationToken);
    public Task CheckProfessorAssignmentAsync(long courseOfferingId, long professorId, IReadOnlyCollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default)
        => CheckAsync(courseOfferingId, slots, professorId, cancellationToken);
    private async Task CheckAsync(long offeringId, IReadOnlyCollection<CourseOfferingScheduleSlotDto> slots, long? professorId, CancellationToken cancellationToken)
    {
        if (slots.Count == 0) return;
        var professors = await repository.GetDataAsync(offeringId, professorId, cancellationToken);
        foreach (var professor in professors)
            foreach (var slot in slots)
            {
                if (professor.OtherSchedules.Any(s => s.DayOfWeek == slot.DayOfWeek
                    && s.StartTime < slot.EndTime && slot.StartTime < s.EndTime))
                    throw new BusinessException($"Schedule conflict for professor {professor.FullName}.");
                if (professor.Availability.Count > 0 && !professor.Availability.Any(a => a.DayOfWeek == slot.DayOfWeek
                    && a.StartTime <= slot.StartTime && slot.EndTime <= a.EndTime))
                    throw new BusinessException($"Schedule is outside availability for professor {professor.FullName}.");
            }
    }
}
