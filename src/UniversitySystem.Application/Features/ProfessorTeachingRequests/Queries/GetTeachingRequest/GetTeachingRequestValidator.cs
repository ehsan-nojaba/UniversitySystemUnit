using FluentValidation;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Queries.GetTeachingRequest;

/// <summary>
/// اعتبارسنجی ورودی عملیات «مشاهده درخواست تدریس استاد جاری» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class GetTeachingRequestValidator : AbstractValidator<GetTeachingRequestQuery>
{
    public GetTeachingRequestValidator()
    {
        RuleFor(x => x.AcademicTermId).GreaterThan(0);
    }
}
