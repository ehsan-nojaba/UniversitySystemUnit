using UniversitySystem.Application.Features.AdminPreRegistration.Queries.GetCourseDemandSummary;

namespace UniversitySystem.UnitTests.StudentPreRegistration;

public class GetCourseDemandSummaryQueryValidatorTests
{
    private readonly GetCourseDemandSummaryQueryValidator _validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(12345)]
    public void Validate_WhenAcademicTermIdIsPositive_PassesValidation(long termId)
    {
        // Arrange
        var query = new GetCourseDemandSummaryQuery { AcademicTermId = termId };

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-500)]
    public void Validate_WhenAcademicTermIdIsZeroOrNegative_FailsValidation(long termId)
    {
        // Arrange
        var query = new GetCourseDemandSummaryQuery { AcademicTermId = termId };

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(GetCourseDemandSummaryQuery.AcademicTermId));
    }
}
