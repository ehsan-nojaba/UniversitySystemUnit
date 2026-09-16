using FluentValidation;

namespace UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;

/// <summary>
/// اعتبارسنجی ورودی عملیات «مشاهده استادهای تخصیص‌یافته» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class GetTeachingAssignmentsQueryValidator : AbstractValidator<GetTeachingAssignmentsQuery>
{
    public GetTeachingAssignmentsQueryValidator()
    {
        RuleFor(x => x.CourseOfferingId)
            .GreaterThan(0)
            .WithMessage("شناسه ارائه درس نامعتبر است.");
    }
}
