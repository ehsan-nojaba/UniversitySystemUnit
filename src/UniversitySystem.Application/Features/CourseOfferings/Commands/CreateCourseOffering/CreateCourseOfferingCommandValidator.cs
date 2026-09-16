using FluentValidation;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.CreateCourseOffering;

/// <summary>
/// اعتبارسنجی ورودی عملیات «ایجاد ارائه درس با ظرفیت مشخص» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class CreateCourseOfferingCommandValidator : AbstractValidator<CreateCourseOfferingCommand>
{
    public CreateCourseOfferingCommandValidator()
    {
        RuleFor(x => x.AcademicTermId)
            .GreaterThan(0)
            .WithMessage("شناسه ترم تحصیلی نامعتبر است.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("شناسه درس نامعتبر است.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("ظرفیت کلاس باید یک عدد مثبت باشد.");
    }
}
