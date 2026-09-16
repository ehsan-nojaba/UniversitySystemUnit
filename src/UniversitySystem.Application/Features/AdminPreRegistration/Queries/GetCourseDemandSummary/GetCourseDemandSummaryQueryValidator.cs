using FluentValidation;

namespace UniversitySystem.Application.Features.AdminPreRegistration.Queries.GetCourseDemandSummary;

/// <summary>
/// اعتبارسنجی ورودی عملیات «دریافت تقاضای درس از درخواست‌های ارسال‌شده» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class GetCourseDemandSummaryQueryValidator
    : AbstractValidator<GetCourseDemandSummaryQuery>
{
    public GetCourseDemandSummaryQueryValidator()
    {
        RuleFor(x => x.AcademicTermId)
            .GreaterThan(0)
            .WithMessage("شناسه ترم تحصیلی نامعتبر است.");
    }
}
