using UniversitySystem.Application.Features.AdminPlanning.Queries.GetAcademicPlanningOverview;
using Xunit;

namespace UniversitySystem.UnitTests.AdminPlanning;

public class GetAcademicPlanningOverviewQueryValidatorTests
{
    private readonly GetAcademicPlanningOverviewQueryValidator _validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(12345)]
    public void Validate_WhenAcademicTermIdIsPositive_PassesValidation(long termId)
    {
        var query = new GetAcademicPlanningOverviewQuery { AcademicTermId = termId };
        var result = _validator.Validate(query);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-500)]
    public void Validate_WhenAcademicTermIdIsZeroOrNegative_FailsValidation(long termId)
    {
        var query = new GetAcademicPlanningOverviewQuery { AcademicTermId = termId };
        var result = _validator.Validate(query);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(GetAcademicPlanningOverviewQuery.AcademicTermId));
    }
}
