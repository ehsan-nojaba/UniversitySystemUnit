using FluentValidation;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.SaveCourseOfferingSchedule;
/// <summary>
/// اعتبارسنجی ورودی عملیات «ذخیره زمان‌بندی ارائه با کنترل تداخل» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class SaveCourseOfferingScheduleCommandValidator : AbstractValidator<SaveCourseOfferingScheduleCommand>
{
    public SaveCourseOfferingScheduleCommandValidator()
    {
        RuleFor(x => x.CourseOfferingId).GreaterThan(0).WithMessage("شناسه ارائه درس نامعتبر است.");
        RuleForEach(x => x.Slots).ChildRules(slot =>
        {
            slot.RuleFor(s => s.DayOfWeek).IsInEnum().WithMessage("روز هفته نامعتبر است.");
            slot.RuleFor(s => s.StartTime).LessThan(s => s.EndTime).WithMessage("زمان شروع باید قبل از زمان پایان باشد.");
        });
        RuleFor(x => x.Slots).NotNull().Must(NotHaveDuplicateSlots).WithMessage("اسلات تکراری مجاز نیست.").Must(NotHaveOverlappingSlots).WithMessage("در یک ارائه دو اسلات متداخل در یک روز مجاز نیست.");
    }

    private static bool NotHaveDuplicateSlots(List<CourseOfferingScheduleSlotDto>? slots)
    {
        if (slots is null || slots.Count <= 1)
        {
            return true;
        }

        return !slots.GroupBy(s => new { s.DayOfWeek, s.StartTime, s.EndTime }).Any(g => g.Count() > 1);
    }

    private static bool NotHaveOverlappingSlots(List<CourseOfferingScheduleSlotDto>? slots)
    {
        if (slots is null || slots.Count <= 1)
        {
            return true;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = i + 1; j < slots.Count; j++)
            {
                if (slots[i].DayOfWeek == slots[j].DayOfWeek)
                {
                    // Overlap rule: s1.StartTime < s2.EndTime && s2.StartTime < s1.EndTime
                    if (slots[i].StartTime < slots[j].EndTime && slots[j].StartTime < slots[i].EndTime)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}
