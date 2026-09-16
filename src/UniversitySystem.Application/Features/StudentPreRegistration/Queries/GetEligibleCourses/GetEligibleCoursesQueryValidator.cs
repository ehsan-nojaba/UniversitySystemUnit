using FluentValidation;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

/// <summary>
/// اعتبارسنجی ورودی عملیات «دریافت درس‌های مجاز دانشجو» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class GetEligibleCoursesQueryValidator : AbstractValidator<GetEligibleCoursesQuery>
{
    public GetEligibleCoursesQueryValidator()
    {
        RuleFor(x => x.AcademicTermId)
            .GreaterThan(0)
            .WithMessage("شناسه ترم تحصیلی معتبر نیست.");
    }
}
