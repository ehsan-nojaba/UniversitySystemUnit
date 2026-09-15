using UniversitySystem.Application.Features.CourseOfferings.Commands.SaveCourseOfferingSchedule;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.UnitTests.CourseOfferings;

public class SaveCourseOfferingScheduleCommandValidatorTests
{
    private readonly SaveCourseOfferingScheduleCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenCommandIsValid_PassesValidation()
    {
        // Arrange
        var command = new SaveCourseOfferingScheduleCommand
        {
            CourseOfferingId = 1,
            Slots =
            [
                new CourseOfferingScheduleSlotDto
                {
                    DayOfWeek = DayOfWeek.Saturday,
                    StartTime = new TimeOnly(8, 0),
                    EndTime = new TimeOnly(10, 0)
                },
                new CourseOfferingScheduleSlotDto
                {
                    DayOfWeek = DayOfWeek.Monday,
                    StartTime = new TimeOnly(10, 0),
                    EndTime = new TimeOnly(12, 0)
                }
            ]
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenEmptySlots_PassesValidation()
    {
        // Arrange
        var command = new SaveCourseOfferingScheduleCommand
        {
            CourseOfferingId = 1,
            Slots = []
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenCourseOfferingIdIsInvalid_FailsValidation(long id)
    {
        // Arrange
        var command = new SaveCourseOfferingScheduleCommand
        {
            CourseOfferingId = id,
            Slots = []
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(SaveCourseOfferingScheduleCommand.CourseOfferingId));
    }

    [Fact]
    public void Validate_WhenDayOfWeekIsInvalid_FailsValidation()
    {
        // Arrange
        var command = new SaveCourseOfferingScheduleCommand
        {
            CourseOfferingId = 1,
            Slots =
            [
                new CourseOfferingScheduleSlotDto
                {
                    DayOfWeek = (DayOfWeek)99,
                    StartTime = new TimeOnly(8, 0),
                    EndTime = new TimeOnly(10, 0)
                }
            ]
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("DayOfWeek"));
    }

    [Fact]
    public void Validate_WhenStartTimeEqualsOrAfterEndTime_FailsValidation()
    {
        // Arrange
        var command = new SaveCourseOfferingScheduleCommand
        {
            CourseOfferingId = 1,
            Slots =
            [
                new CourseOfferingScheduleSlotDto
                {
                    DayOfWeek = DayOfWeek.Saturday,
                    StartTime = new TimeOnly(10, 0),
                    EndTime = new TimeOnly(8, 0)
                }
            ]
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("StartTime"));
    }

    [Fact]
    public void Validate_WhenSlotsContainDuplicates_FailsValidation()
    {
        // Arrange
        var command = new SaveCourseOfferingScheduleCommand
        {
            CourseOfferingId = 1,
            Slots =
            [
                new CourseOfferingScheduleSlotDto
                {
                    DayOfWeek = DayOfWeek.Saturday,
                    StartTime = new TimeOnly(8, 0),
                    EndTime = new TimeOnly(10, 0)
                },
                new CourseOfferingScheduleSlotDto
                {
                    DayOfWeek = DayOfWeek.Saturday,
                    StartTime = new TimeOnly(8, 0),
                    EndTime = new TimeOnly(10, 0)
                }
            ]
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("اسلات تکراری"));
    }

    [Fact]
    public void Validate_WhenSlotsOverlapWithinSameOffering_FailsValidation()
    {
        // Arrange
        var command = new SaveCourseOfferingScheduleCommand
        {
            CourseOfferingId = 1,
            Slots =
            [
                new CourseOfferingScheduleSlotDto
                {
                    DayOfWeek = DayOfWeek.Saturday,
                    StartTime = new TimeOnly(8, 0),
                    EndTime = new TimeOnly(10, 0)
                },
                new CourseOfferingScheduleSlotDto
                {
                    DayOfWeek = DayOfWeek.Saturday,
                    StartTime = new TimeOnly(9, 30),
                    EndTime = new TimeOnly(11, 30)
                }
            ]
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("متداخل"));
    }

    [Fact]
    public void Validate_WhenSlotsAreConsecutiveOnSameDay_PassesValidation()
    {
        // Arrange
        var command = new SaveCourseOfferingScheduleCommand
        {
            CourseOfferingId = 1,
            Slots =
            [
                new CourseOfferingScheduleSlotDto
                {
                    DayOfWeek = DayOfWeek.Saturday,
                    StartTime = new TimeOnly(8, 0),
                    EndTime = new TimeOnly(10, 0)
                },
                new CourseOfferingScheduleSlotDto
                {
                    DayOfWeek = DayOfWeek.Saturday,
                    StartTime = new TimeOnly(10, 0),
                    EndTime = new TimeOnly(12, 0)
                }
            ]
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
