using FluentValidation;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;

/// <summary>
/// اعتبارسنجی ورودی عملیات «ویرایش ظرفیت و وضعیت ارائه» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class UpdateCourseOfferingCommandValidator : AbstractValidator<UpdateCourseOfferingCommand>
{
    public UpdateCourseOfferingCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("شناسه ارائه درس نامعتبر است.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("ظرفیت کلاس باید یک عدد مثبت باشد.");
    }
}
