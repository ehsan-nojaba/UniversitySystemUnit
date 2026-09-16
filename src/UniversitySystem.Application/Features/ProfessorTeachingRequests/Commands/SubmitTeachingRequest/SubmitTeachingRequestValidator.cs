using FluentValidation;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SubmitTeachingRequest;

/// <summary>
/// اعتبارسنجی ورودی عملیات «ارسال نهایی درخواست تدریس» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class SubmitTeachingRequestValidator : AbstractValidator<SubmitTeachingRequestCommand>
{
    public SubmitTeachingRequestValidator() => RuleFor(x => x.AcademicTermId).GreaterThan(0);
}
