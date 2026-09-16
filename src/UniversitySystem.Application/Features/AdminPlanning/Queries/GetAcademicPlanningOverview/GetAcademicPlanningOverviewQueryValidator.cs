using FluentValidation;

namespace UniversitySystem.Application.Features.AdminPlanning.Queries.GetAcademicPlanningOverview;

/// <summary>
/// اعتبارسنجی ورودی عملیات «دریافت نمای برنامه‌ریزی آموزش» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class GetAcademicPlanningOverviewQueryValidator : AbstractValidator<GetAcademicPlanningOverviewQuery>
{
    public GetAcademicPlanningOverviewQueryValidator()
    {
        RuleFor(x => x.AcademicTermId)
            .GreaterThan(0)
            .WithMessage("شناسه ترم تحصیلی نامعتبر است.");
    }
}
