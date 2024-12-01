using Services.Applications.ValidationHelpers;
using Shouldly;
using Xunit;

namespace Services.Applications.Tests.ValidationHelperTests;

public class AgeValidationTests
{
    [Theory]
    [InlineData(21, 0, true, "")]
    [InlineData(21, -1, true, "")]
    [InlineData(21, +1, false, "Must be 21 years old or less.")]
    public void Given_OffsetDaysFromDateOfBirth_MaxAge_ShouldReturn_ExpectedResult(
        int maxAgeYears, int offsetDays, bool expectedResult, string expectedErrorMessage)
    {
        //Arrange
        var today = DateTime.Today;
        var dob = new DateOnly(today.Year -maxAgeYears, today.Month, today.Day)
            .AddDays(offsetDays *-1);
        
        //Act
        var result = AgeValidation.MaxAge(maxAgeYears, dob);

        //Assert
        result.IsSuccess.ShouldBe(expectedResult);
        result.Error.Description.ShouldBe(expectedErrorMessage);
    }
    
    [Theory]
    [InlineData(18, 0, true, "")]
    [InlineData(18, -1, false, "Must be 18 years or older.")]
    [InlineData(18, +1, true, "")]
    public void Given_OffsetDaysFromDateOfBirth_MinAge_ShouldReturn_ExpectedResult(
        int minAgeYears, int offsetDays, bool expectedResult, string expectedErrorMessage)
    {
        //Arrange
        var today = DateTime.Today;
        var dob = new DateOnly(today.Year -minAgeYears, today.Month, today.Day)
            .AddDays(offsetDays *-1);
        
        //Act
        var result = AgeValidation.MinAge(minAgeYears, dob);

        //Assert
        result.IsSuccess.ShouldBe(expectedResult);
        result.Error.Description.ShouldBe(expectedErrorMessage);
    }
}