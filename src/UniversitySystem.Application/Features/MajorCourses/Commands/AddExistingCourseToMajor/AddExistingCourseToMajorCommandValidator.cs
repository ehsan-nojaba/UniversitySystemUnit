using FluentValidation;

namespace UniversitySystem.Application.Features.MajorCourses.Commands.AddExistingCourseToMajor;

/// <summary>
/// اعتبارسنجی ورودی عملیات «افزودن درس موجود به چارت رشته» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class AddExistingCourseToMajorCommandValidator : AbstractValidator<AddExistingCourseToMajorCommand>
{
    public AddExistingCourseToMajorCommandValidator()
    {
        RuleFor(x => x.MajorId)
            .GreaterThan(0)
            .WithMessage("شناسه رشته نامعتبر است.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("شناسه درس نامعتبر است.");

        RuleFor(x => x.RecommendedTerm)
            .GreaterThan(0)
            .WithMessage("ترم پیشنهادی باید یک عدد مثبت باشد.");
    }
}
