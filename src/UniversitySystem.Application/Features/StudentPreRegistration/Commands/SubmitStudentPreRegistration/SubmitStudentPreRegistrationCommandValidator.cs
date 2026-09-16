using FluentValidation;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SubmitStudentPreRegistration;

/// <summary>
/// اعتبارسنجی ورودی عملیات «ارسال نهایی پیش‌انتخاب» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class SubmitStudentPreRegistrationCommandValidator
    : AbstractValidator<SubmitStudentPreRegistrationCommand>
{
    public SubmitStudentPreRegistrationCommandValidator()
    {
        RuleFor(x => x.AcademicTermId)
            .GreaterThan(0)
            .WithMessage("شناسه ترم تحصیلی نامعتبر است.");
    }
}
