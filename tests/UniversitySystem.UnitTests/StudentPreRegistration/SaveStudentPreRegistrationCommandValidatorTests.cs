using UniversitySystem.Application.Features.StudentPreRegistration.Commands.SaveStudentPreRegistration;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.UnitTests.StudentPreRegistration;

public class SaveStudentPreRegistrationCommandValidatorTests
{
    private readonly SaveStudentPreRegistrationCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenCommandIsValid_PassesValidation()
    {
        // Arrange
        var courses = new List<SelectedCourseItemDto>
        {
            new() { CourseId = 1, Priority = 1 },
            new() { CourseId = 2, Priority = 2 }
        };
        var command = new SaveStudentPreRegistrationCommand { AcademicTermId = 1, Courses = courses };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenAcademicTermIdIsInvalid_FailsValidation(long termId)
    {
        // Arrange
        var courses = new List<SelectedCourseItemDto> { new() { CourseId = 1, Priority = 1 } };
        var command = new SaveStudentPreRegistrationCommand { AcademicTermId = termId, Courses = courses };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(SaveStudentPreRegistrationCommand.AcademicTermId));
    }

    [Fact]
    public void Validate_WhenCoursesEmpty_FailsValidation()
    {
        // Arrange
        var command = new SaveStudentPreRegistrationCommand { AcademicTermId = 1, Courses = [] };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(SaveStudentPreRegistrationCommand.Courses));
    }

    [Fact]
    public void Validate_WhenCoursesContainDuplicates_FailsValidation()
    {
        // Arrange
        var courses = new List<SelectedCourseItemDto>
        {
            new() { CourseId = 5, Priority = 1 },
            new() { CourseId = 5, Priority = 2 }
        };
        var command = new SaveStudentPreRegistrationCommand { AcademicTermId = 1, Courses = courses };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("تکراری"));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, 1)]
    [InlineData(1, -1)]
    public void Validate_WhenCourseItemHasInvalidIdOrPriority_FailsValidation(long courseId, int priority)
    {
        // Arrange
        var courses = new List<SelectedCourseItemDto> { new() { CourseId = courseId, Priority = priority } };
        var command = new SaveStudentPreRegistrationCommand { AcademicTermId = 1, Courses = courses };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }
}
