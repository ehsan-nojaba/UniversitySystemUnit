using FluentValidation;

namespace UniversitySystem.Application.Features.AdminPlanning.Queries.GetAcademicPlanningOverview;

public sealed class GetAcademicPlanningOverviewQueryValidator : AbstractValidator<GetAcademicPlanningOverviewQuery>
{
    public GetAcademicPlanningOverviewQueryValidator()
    {
        RuleFor(x => x.AcademicTermId)
            .GreaterThan(0)
            .WithMessage("شناسه ترم تحصیلی نامعتبر است.");
    }
}
