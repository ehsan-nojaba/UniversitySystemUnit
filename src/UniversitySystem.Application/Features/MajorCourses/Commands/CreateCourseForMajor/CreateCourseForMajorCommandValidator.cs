using FluentValidation;

namespace UniversitySystem.Application.Features.MajorCourses.Commands.CreateCourseForMajor;

/// <summary>
/// اعتبارسنجی ورودی عملیات «ساخت درس جدید و افزودن به چارت رشته» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class CreateCourseForMajorCommandValidator : AbstractValidator<CreateCourseForMajorCommand>
{
    public CreateCourseForMajorCommandValidator()
    {
        RuleFor(x => x.MajorId)
            .GreaterThan(0)
            .WithMessage("شناسه رشته نامعتبر است.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("کد درس الزامی است.")
            .MaximumLength(50)
            .WithMessage("کد درس نمی‌تواند بیش از ۵۰ کاراکتر باشد.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("عنوان درس الزامی است.")
            .MaximumLength(200)
            .WithMessage("عنوان درس نمی‌تواند بیش از ۲۰۰ کاراکتر باشد.");

        RuleFor(x => x.Credits)
            .InclusiveBetween(1, 6)
            .WithMessage("تعداد واحد باید بین ۱ تا ۶ باشد.");

        RuleFor(x => x.RecommendedTerm)
            .GreaterThan(0)
            .WithMessage("ترم پیشنهادی باید یک عدد مثبت باشد.");
    }
}
