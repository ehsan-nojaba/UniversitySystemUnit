using FluentValidation;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Queries.GetTeachingRequestSummary;

/// <summary>
/// اعتبارسنجی ورودی عملیات «جمع‌بندی درخواست‌های ارسال‌شده استادها» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class GetTeachingRequestSummaryValidator : AbstractValidator<GetTeachingRequestSummaryQuery>
{
    public GetTeachingRequestSummaryValidator()
    {
        RuleFor(x => x.AcademicTermId).GreaterThan(0);
    }
}
