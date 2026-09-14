using UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

namespace UniversitySystem.UnitTests.StudentPreRegistration;

public class GetEligibleCoursesQueryValidatorTests
{
    private readonly GetEligibleCoursesQueryValidator _validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(999)]
    public void Validate_WhenAcademicTermIdIsGreaterThanZero_IsValid(long termId)
    {
        // Arrange
        var query = new GetEligibleCoursesQuery(termId);

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-50)]
    public void Validate_WhenAcademicTermIdIsZeroOrNegative_FailsValidation(long termId)
    {
        // Arrange
        var query = new GetEligibleCoursesQuery(termId);

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(GetEligibleCoursesQuery.AcademicTermId));
    }
}
