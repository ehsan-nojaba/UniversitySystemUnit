using FluentValidation;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

/// <summary>
/// Validates <see cref="GetEligibleCoursesQuery"/> parameters.
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
