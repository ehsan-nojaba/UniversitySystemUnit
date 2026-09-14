using FluentValidation;

namespace UniversitySystem.Application.Features.AdminPreRegistration.Queries.GetCourseDemandSummary;

/// <summary>
/// Validator for <see cref="GetCourseDemandSummaryQuery"/>.
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
