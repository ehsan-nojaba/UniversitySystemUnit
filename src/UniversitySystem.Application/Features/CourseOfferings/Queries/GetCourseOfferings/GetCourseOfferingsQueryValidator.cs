using FluentValidation;

namespace UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;

public sealed class GetCourseOfferingsQueryValidator : AbstractValidator<GetCourseOfferingsQuery>
{
    public GetCourseOfferingsQueryValidator()
    {
        RuleFor(x => x.AcademicTermId)
            .GreaterThan(0)
            .WithMessage("شناسه ترم تحصیلی نامعتبر است.");
    }
}
