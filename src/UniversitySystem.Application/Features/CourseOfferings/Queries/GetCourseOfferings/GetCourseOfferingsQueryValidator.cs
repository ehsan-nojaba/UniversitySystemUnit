using FluentValidation;

namespace UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;

/// <summary>
/// اعتبارسنجی ورودی عملیات «دریافت ارائه‌های یک ترم» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class GetCourseOfferingsQueryValidator : AbstractValidator<GetCourseOfferingsQuery>
{
    public GetCourseOfferingsQueryValidator()
    {
        RuleFor(x => x.AcademicTermId)
            .GreaterThan(0)
            .WithMessage("شناسه ترم تحصیلی نامعتبر است.");
    }
}
