using FluentValidation;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SaveAvailability;

/// <summary>
/// اعتبارسنجی ورودی عملیات «ثبت زمان‌های آزاد استاد» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class SaveAvailabilityValidator : AbstractValidator<SaveAvailabilityCommand>
{
    public SaveAvailabilityValidator()
    {
        RuleFor(x => x.AcademicTermId).GreaterThan(0);
        RuleFor(x => x.Availability).NotNull().Must(a => a is null || !a.Select((x, i) => a.Skip(i + 1)
            .Any(y => x.DayOfWeek == y.DayOfWeek && x.StartTime < y.EndTime && y.StartTime < x.EndTime)).Any(v => v))
            .WithMessage("Availability intervals cannot overlap or repeat.");
        RuleForEach(x => x.Availability).ChildRules(a =>
        {
            a.RuleFor(x => x.DayOfWeek).IsInEnum(); a.RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime);
        });
    }
}
