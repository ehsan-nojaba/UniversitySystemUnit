using FluentValidation;

namespace UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;

public sealed class GetTeachingAssignmentsQueryValidator : AbstractValidator<GetTeachingAssignmentsQuery>
{
    public GetTeachingAssignmentsQueryValidator()
    {
        RuleFor(x => x.CourseOfferingId)
            .GreaterThan(0)
            .WithMessage("شناسه ارائه درس نامعتبر است.");
    }
}
